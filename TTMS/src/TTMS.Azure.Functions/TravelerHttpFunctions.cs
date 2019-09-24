using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using TTMS.Common.DTO.Extensions;
using TTMS.Common.Enums;
using TTMS.Common.Entities;
using TTMS.Common.Entities.Extensions;
using TTMS.Common.DTO;

namespace TTMS.Azure.Functions
{
    public static class TravelerHttpFunctions
    {
        #region Table Reader

        [FunctionName(nameof(GetAllTravelersAsync))]
        public static async Task<IActionResult> GetAllTravelersAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "")] HttpRequest req,
            [Table("traveler")]  CloudTable table,
            ILogger logger)
        {
            logger.LogDebug("{Class}.{Method}", nameof(TravelerHttpFunctions), nameof(GetAllTravelersAsync));

            var result = await table.ExecuteQueryAsync<Traveler>();

            return new OkObjectResult(result?.CreateResponse());
        }

        [FunctionName(nameof(GetTravelerByIdAsync))]
        public static async Task<IActionResult> GetTravelerByIdAsync(
         [HttpTrigger(AuthorizationLevel.Function, "get", Route = "{id}")] HttpRequest req,
         [Table("traveler")] CloudTable table,
         Guid id,
         ILogger logger)
        {
            logger.LogDebug("{Class}.{Method}", nameof(TravelerHttpFunctions), nameof(GetTravelerByIdAsync));

            var query = new TableQuery<Traveler>().Where(
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id.ToString()));

            var result = (await table.ExecuteQueryAsync(query))?.FirstOrDefault();

            if (result == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(result.CreateResponse());
        }

        [FunctionName(nameof(GetTravelerByTypeAsync))]
        public static async Task<IActionResult> GetTravelerByTypeAsync(
         [HttpTrigger(AuthorizationLevel.Function, "get", Route = "type/{type}")] HttpRequest req,
         [Table("traveler")] CloudTable table,
         TravelerType travelerType,
         ILogger logger)
        {
            logger.LogDebug("{Class}.{Method}", nameof(TravelerHttpFunctions), nameof(GetTravelerByTypeAsync));

            var query = new TableQuery<Traveler>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, travelerType.ToString()));

            var results = await table.ExecuteQueryAsync(query);

            return new OkObjectResult(results?.CreateResponse());
        }

        #endregion

        #region Table Writer

        [FunctionName(nameof(CreateTravelerAsync))]
        public static async Task<IActionResult> CreateTravelerAsync(
         [HttpTrigger(AuthorizationLevel.Function, "post", Route = "")] HttpRequest req,
         [Table("traveler")] CloudTable table,
         TravelerRequest traveler,
         ILogger logger)
        {
            logger.LogDebug("{Class}.{Method}", nameof(TravelerHttpFunctions), nameof(CreateTravelerAsync));

            if (traveler.Id == default)
            {
                traveler.Id = Guid.NewGuid();
            }

            var operation = TableOperation.Insert(traveler.ToEntity());
            var results = await table.ExecuteAsync(operation);

            return new CreatedAtRouteResult(new { id = traveler.Id }, traveler.ToEntity().CreateResponse());
        }

        [FunctionName(nameof(UpdateTravelerAsync))]
        public static async Task<IActionResult> UpdateTravelerAsync(
         [HttpTrigger(AuthorizationLevel.Function, "post", Route = "{id:Guid}")] HttpRequest req,
         [Table("traveler")] CloudTable table,
         Guid id,
         TravelerRequest traveler,
         ILogger logger)
        {
            logger.LogDebug("{Class}.{Method}", nameof(TravelerHttpFunctions), nameof(UpdateTravelerAsync));

            if (id != traveler.Id)
            {
                var msg = "It's not allowed to change traveler ID.";
                logger.LogWarning("BAD REQUEST: {msg} => {@Request}", msg, traveler);
                return new BadRequestObjectResult(msg);
            }

            var entity = traveler.ToEntity();
            entity.ETag = "*";
            var operation = TableOperation.Replace(entity);
            var result = await table.ExecuteAsync(operation);

            if (result.HttpStatusCode == (int)HttpStatusCode.NotFound)
            {
                return new NotFoundResult();
            }

            return new OkResult();
        }

        [FunctionName(nameof(DeleteTravelerAsync))]
        public static async Task<IActionResult> DeleteTravelerAsync(
         [HttpTrigger(AuthorizationLevel.Function, "post", Route = "{id:Guid}")] HttpRequest req,
         [Table("traveler")] CloudTable table,
         Guid id,
         ILogger logger)
        {
            logger.LogDebug("{Class}.{Method}", nameof(TravelerHttpFunctions), nameof(DeleteTravelerAsync));

            var query = new TableQuery<Traveler>()
                            .Where(TableQuery.GenerateFilterCondition(
                                    nameof(Traveler.RowKey),
                                    QueryComparisons.Equal,
                                    id.ToString()));

            var entityToDelete = (await table.ExecuteQueryAsync(query))?.FirstOrDefault();

            if (entityToDelete == null)
            {
                return new NotFoundResult();
            }

            var operation = TableOperation.Delete(entityToDelete);
            await table.ExecuteAsync(operation);

            return new OkResult();
        }

        #endregion
    }
}

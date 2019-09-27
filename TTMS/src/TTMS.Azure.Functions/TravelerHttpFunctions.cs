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
using TTMS.Common.DTO;
using TTMS.Common.DTO.Extensions;
using TTMS.Common.Entities;
using TTMS.Common.Entities.Extensions;

namespace TTMS.Azure.Functions
{
    public static class TravelerHttpFunctions
    {
        #region Table Reader

        [FunctionName(nameof(GetAllTravelersAsync))]
        public static async Task<IActionResult> GetAllTravelersAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "travelers")] HttpRequest req,
            [Table("traveler")]  CloudTable table,
            ILogger logger)
        {
            try
            {
                logger.LogInformation("{Class}.{Method}", nameof(TravelerHttpFunctions), nameof(GetAllTravelersAsync));

                var result = await table.ExecuteQueryAsync<Traveler>();

                logger.LogDebug("Travelers found: {count}", result.Count());
                return new OkObjectResult(result.CreateResponse());
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        [FunctionName(nameof(GetTravelerByIdAsync))]
        public static async Task<IActionResult> GetTravelerByIdAsync(
         [HttpTrigger(AuthorizationLevel.Function, "get", Route = "travelers/{id:Guid}")] HttpRequest req,
         [Table("traveler")] CloudTable table,
         string id,
         ILogger logger)
        {
            try
            {
                logger.LogDebug("{Class}.{Method}: {id}", nameof(TravelerHttpFunctions), nameof(GetTravelerByIdAsync), id);

                var query = new TableQuery<Traveler>().Where(
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id));

                var result = (await table.ExecuteQueryAsync(query))?.FirstOrDefault();

                if (result == null)
                {
                    logger.LogWarning("No traveler found with ID {id}", id);
                    return new NotFoundResult();
                }

                return new OkObjectResult(result.CreateResponse());
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        [FunctionName(nameof(GetTravelerByTypeAsync))]
        public static async Task<IActionResult> GetTravelerByTypeAsync(
         [HttpTrigger(AuthorizationLevel.Function, "get", Route = "travelers/type/{type}")] HttpRequest req,
         [Table("traveler")] CloudTable table,
         string type,
         ILogger logger)
        {
            try
            {
                logger.LogDebug("{Class}.{Method}: {type}", nameof(TravelerHttpFunctions), nameof(GetTravelerByTypeAsync), type);

                var query = new TableQuery<Traveler>().Where(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, type));

                var results = await table.ExecuteQueryAsync(query);

                logger.LogDebug("Travelers found: {count}", results.Count());
                return new OkObjectResult(results?.CreateResponse());
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        #endregion

        #region Table Writer

        [FunctionName(nameof(CreateTravelerAsync))]
        public static async Task<IActionResult> CreateTravelerAsync(
         [HttpTrigger(AuthorizationLevel.Function, "post", Route = "travelers")] HttpRequest req,
         [Table("traveler")] CloudTable table,
         ILogger logger)
        {
            try
            {
                logger.LogInformation("{Class}.{Method}", nameof(TravelerHttpFunctions), nameof(CreateTravelerAsync));

                var traveler = await req.DeserializeAsync<TravelerRequest>();

                var entity = traveler.ToEntity();

                var result = await TableStorageHelper.CreateTravelerAsync(table, entity, logger);

                if (result.HttpStatusCode == (int)HttpStatusCode.Created)
                {
                    return new CreatedAtRouteResult(new { id = entity.RowKey }, entity.CreateResponse());
                }
                else
                {
                    return new StatusCodeResult(result.HttpStatusCode);
                }
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        [FunctionName(nameof(UpdateTravelerAsync))]
        public static async Task<IActionResult> UpdateTravelerAsync(
         [HttpTrigger(AuthorizationLevel.Function, "put", Route = "travelers/{id}")] HttpRequest req,
         [Table("traveler")] CloudTable table,
         string id,
         ILogger logger)
        {
            try
            {
                logger.LogDebug("{Class}.{Method}", nameof(TravelerHttpFunctions), nameof(UpdateTravelerAsync));

                var entity = (await req.DeserializeAsync<TravelerRequest>()).ToEntity();

                if (id != entity.RowKey)
                {
                    logger.LogError("It's not allowed to change entity ID");
                    return new BadRequestResult();
                }

                var result = await TableStorageHelper.UpdateTravelerAsync(table, entity, logger);

                return new StatusCodeResult(result.HttpStatusCode);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        [FunctionName(nameof(DeleteTravelerAsync))]
        public static async Task<IActionResult> DeleteTravelerAsync(
         [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "travelers/{id}")] HttpRequest req,
         [Table("traveler")] CloudTable table,
         string id,
         ILogger logger)
        {
            try
            {
                logger.LogInformation("{Class}.{Method}: {id}", nameof(TravelerHttpFunctions), nameof(DeleteTravelerAsync), id);

                var query = new TableQuery<Traveler>()
                                .Where(TableQuery.GenerateFilterCondition(
                                        nameof(Traveler.RowKey),
                                        QueryComparisons.Equal,
                                        id.ToString()));

                var entityToDelete = (await table.ExecuteQueryAsync(query))?.FirstOrDefault();

                if (entityToDelete == null)
                {
                    logger.LogWarning("HTTP 404: No traveler found with ID {id}", id);
                    return new NotFoundResult();
                }

                var result = await TableStorageHelper.DeleteTravelerAsync(table, id, logger);

                logger.LogDebug("Traveler delete result: HTTP {HttpStatusCode}", result.HttpStatusCode);
                return new StatusCodeResult(result.HttpStatusCode);
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex, ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                throw;
            }
        }

        #endregion
    }
}

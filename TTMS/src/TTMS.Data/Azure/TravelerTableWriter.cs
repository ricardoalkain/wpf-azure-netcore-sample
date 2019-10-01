using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using TTMS.Common.Abstractions;
using TTMS.Common.Entities.Extensions;
using TTMS.Common.Models;
using TTMS.Data.Abstractions;
using Entities = TTMS.Common.Entities;

namespace TTMS.Data.Azure
{
    public class TravelerTableWriter : BaseAzureTableProvider<Guid, Entities.Traveler>, ITravelerWriter
    {
        public TravelerTableWriter(
            ILogger<TravelerTableWriter> logger,
            IConfiguration configuration,
            IAzureCloudFactory cloudFactory)
            : base(logger, nameof(Traveler), configuration, cloudFactory)
        {
        }

        public async Task<Traveler> CreateAsync(Traveler traveler)
        {
            logger.LogDebug("{Method} => {@Traveler}", nameof(CreateAsync), traveler);

            if (traveler == null)
            {
                var ex = new ArgumentNullException(nameof(traveler), "Entity can't be null");
                logger.LogError(ex, "{Method} error", nameof(CreateAsync));
                throw ex;
            }

            if (traveler.Id == default)
            {
                traveler.Id = Guid.NewGuid();
            }

            var entity = traveler.ToEntity();
            var operation = TableOperation.Insert(entity);
            await table.ExecuteAsync(operation);
            return traveler;
        }

        public async Task DeleteAsync(Guid id)
        {
            logger.LogDebug("{Method} => {id}", nameof(DeleteAsync), id);

            var query = TableQuery.GenerateFilterCondition(
                                    nameof(Entities.Traveler.RowKey),
                                    QueryComparisons.Equal,
                                    id.ToString());

            var entityToDelete = (await ExecuteQueryAsync(query))?.FirstOrDefault();
            if (entityToDelete != null)
            {
                var operation = TableOperation.Delete(entityToDelete);
                await table.ExecuteAsync(operation);
            }
        }

        public async Task UpdateAsync(Traveler traveler)
        {
            logger.LogDebug("{Method} => {@Traveler}", nameof(UpdateAsync), traveler);

            if (traveler == null)
            {
                var ex = new ArgumentNullException(nameof(traveler), "Entity can't be null");
                logger.LogError(ex, "{Method} error", nameof(CreateAsync));
                throw ex;
            }

            var entity = traveler.ToEntity();
            entity.ETag = "*";
            var operation = TableOperation.Replace(entity);
            await table.ExecuteAsync(operation);
        }
    }
}

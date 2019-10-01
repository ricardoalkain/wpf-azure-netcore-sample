using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using TTMS.Data.Abstractions;

namespace TTMS.Data.Azure
{
    public abstract class BaseAzureTableProvider<TKey, TEntity> where TEntity : TableEntity, new()
    {
        protected readonly ILogger logger;
        protected CloudTable table;

        public BaseAzureTableProvider(
            ILogger logger,
            string tableName,
            IConfiguration configuration,
            IAzureCloudFactory cloudFactory)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            logger.LogInformation("Initializing Azure Cloud Table client...");
            table = cloudFactory.CreateCloudTable(configuration["DbConnectionString"], tableName);

            logger.LogInformation("Creating table '{table}' if not exists...", tableName);
            Task.Run(async () => await table.CreateIfNotExistsAsync()).Wait();
        }


        protected async Task<IEnumerable<TEntity>> ExecuteQueryAsync(string query = null)
        {
            logger.LogDebug("Executing query: {query}", query);

            var tableQuery = new TableQuery<TEntity>().Where(query);
            return await ExecuteQueryAsync(tableQuery);
        }

        protected async Task<IEnumerable<TEntity>> ExecuteQueryAsync(TableQuery<TEntity> tableQuery)
        {
            var continuationToken = default(TableContinuationToken);
            var results = new List<TEntity>();
            int remaining;
            int total = 0;

            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);
                results.AddRange(queryResult.Results);
                continuationToken = queryResult.ContinuationToken;

                if (total == 0)
                {
                    total = tableQuery.TakeCount.GetValueOrDefault(results.Count);
                }
                remaining = total - results.Count;

                logger.LogDebug("Fetched {Current}/{Total}", results.Count, total);

            } while (continuationToken != null && remaining > 0);

            return results;
        }
    }
}
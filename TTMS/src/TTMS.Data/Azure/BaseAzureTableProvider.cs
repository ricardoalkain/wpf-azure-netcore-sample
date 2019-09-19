using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace TTMS.Data.Azure
{
    public abstract class BaseAzureTableProvider<TKey, TEntity> where TEntity : TableEntity, new()
    {
        private readonly ILogger logger;
        protected CloudTable table;

        public BaseAzureTableProvider(ILogger logger, string tableName, string connectionString)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            logger.LogInformation("Initializing Azure Cloud Table client...");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            table = tableClient.GetTableReference(tableName);

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

            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);
                results.AddRange(queryResult.Results);
                continuationToken = queryResult.ContinuationToken;
                remaining = tableQuery.TakeCount.Value - results.Count;

                logger.LogDebug("Fetched {Current} from {Total}", results.Count, tableQuery.TakeCount.Value);

            } while (continuationToken != null && remaining > 0);

            return results;
        }
    }
}
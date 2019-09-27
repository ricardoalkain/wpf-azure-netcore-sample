using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using TTMS.Common.Entities;

namespace TTMS.Azure.Functions
{
    public static class TableStorageHelper
    {
        #region Table Writer helper functions

        private static readonly string UndefinedRowKey = default(Guid).ToString();

        public static async Task<TableResult> CreateTravelerAsync(CloudTable table, Traveler traveler, ILogger logger)
        {
            if (string.IsNullOrEmpty(traveler.RowKey) || traveler.RowKey.CompareTo(UndefinedRowKey) == 0)
            {
                traveler.RowKey = Guid.NewGuid().ToString();
                logger.LogInformation("New traveler key: {RowKey}", traveler.RowKey);
            }

            var operation = TableOperation.Insert(traveler);
            var result = await table.ExecuteAsync(operation);

            if (result.HttpStatusCode == (int)HttpStatusCode.Created)
            {
                logger.LogDebug("New traveler created: {@traveler}", traveler);
            }
            else
            {
                logger.LogError("Database operation failed [HTTP {HttpStatusCode}]", result.HttpStatusCode);
            }

            return result;
        }

        public static async Task<TableResult> UpdateTravelerAsync(CloudTable table, Traveler traveler, ILogger logger)
        {
            traveler.ETag = "*";
            var operation = TableOperation.Replace(traveler);
            return await table.ExecuteAsync(operation);
        }

        public static async Task<TableResult> DeleteTravelerAsync(CloudTable table, string rowKey, ILogger logger)
        {
            var query = new TableQuery<Traveler>()
                            .Where(TableQuery.GenerateFilterCondition(
                                    nameof(Traveler.RowKey),
                                    QueryComparisons.Equal,
                                    rowKey));

            var entityToDelete = (await table.ExecuteQueryAsync(query))?.FirstOrDefault();

            if (entityToDelete == null)
            {
                logger.LogInformation("Traveler {RowKey} not found in the database", entityToDelete.RowKey);
                return new TableResult { HttpStatusCode = (int)HttpStatusCode.NotFound };
            }

            var operation = TableOperation.Delete(entityToDelete);
            return await table.ExecuteAsync(operation);
        }

        #endregion
    }
}

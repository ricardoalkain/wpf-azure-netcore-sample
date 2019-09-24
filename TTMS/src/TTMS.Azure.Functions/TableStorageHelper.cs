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

        public static async Task<Traveler> CreateTravelerAsync(CloudTable table, Traveler traveler)
        {
            if (string.IsNullOrEmpty(traveler.RowKey))
            {
                traveler.RowKey = Guid.NewGuid().ToString();
            }

            var operation = TableOperation.Insert(traveler);
            await table.ExecuteAsync(operation);

            return traveler;
        }

        public static async Task<TableResult> UpdateTravelerAsync(CloudTable table, Traveler traveler)
        {
            traveler.ETag = "*";
            var operation = TableOperation.Replace(traveler);
            return await table.ExecuteAsync(operation);
        }

        public static async Task<TableResult> DeleteTravelerAsync(CloudTable table, Guid id)
        {
            var query = new TableQuery<Traveler>()
                            .Where(TableQuery.GenerateFilterCondition(
                                    nameof(Traveler.RowKey),
                                    QueryComparisons.Equal,
                                    id.ToString()));

            var entityToDelete = (await table.ExecuteQueryAsync(query))?.FirstOrDefault();

            if (entityToDelete == null)
            {
                return new TableResult { HttpStatusCode = (int)HttpStatusCode.NotFound };
            }

            var operation = TableOperation.Delete(entityToDelete);
            return await table.ExecuteAsync(operation);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace TTMS.Azure.Functions
{
    public static class FunctionExtensions
    {
        #region Table Storage

        public static async Task<IEnumerable<TEntity>> ExecuteQueryAsync<TEntity>(
            this CloudTable table, TableQuery<TEntity> tableQuery = null) where TEntity : TableEntity, new()
        {
            var continuationToken = default(TableContinuationToken);
            var results = new List<TEntity>();
            int remaining;
            int total = 0;
            tableQuery = tableQuery ?? new TableQuery<TEntity>();

            do
            {
                var queryResult = await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);
                results.AddRange(queryResult.Results);
                continuationToken = queryResult.ContinuationToken;

                if (total == 0)
                {
                    total = tableQuery.TakeCount.GetValueOrDefault();
                }
                remaining = total - results.Count;

            } while (continuationToken != null && remaining > 0);

            return results;
        }

        #endregion

        #region HttpRequest

        public static async Task<T> DeserializeAsync<T>(this HttpRequest req)
        {
            try
            {
                using (var sr = new StreamReader(req.Body))
                {
                    var payload = await sr.ReadToEndAsync();
                    return JsonConvert.DeserializeObject<T>(payload);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Request stream is invalid. Expected {typeof(T).Name} object.", ex);
            }
        }

        #endregion

    }
}

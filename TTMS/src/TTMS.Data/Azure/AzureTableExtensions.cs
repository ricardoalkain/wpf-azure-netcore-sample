using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace TTMS.Data.Azure
{
    public static class AzureTableExtensions
    {
        /// <summary>
        /// Initiates an asynchronous operation to execute a query and return all the results.
        /// </summary>
        /// <param name="tableQuery">A Microsoft.WindowsAzure.Storage.Table.TableQuery representing the query to execute.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for a task to complete.</param>
        //public static async Task<IEnumerable<TEntity>> ExecuteQueryAsync<TEntity>(this CloudTable table, string query,
        //    CancellationToken cancellationToken = default) where TEntity : TableEntity, new()
        //{
        //    var tableQuery = new TableQuery<TEntity>().Where(query);
        //    var continuationToken = default(TableContinuationToken);
        //    var results = new List<TEntity>();
        //    int remaining;

        //    do
        //    {
        //        //Execute the next query segment async.
        //        var queryResult = await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

        //        //Set exact results list capacity with result count.
        //        results.Capacity += queryResult.Results.Count;

        //        //Add segment results to results list.
        //        results.AddRange(queryResult.Results);

        //        continuationToken = queryResult.ContinuationToken;

        //        remaining = tableQuery.TakeCount.Value - results.Count;

        //    } while (continuationToken != null &&
        //             remaining > 0 &&
        //             !cancellationToken.IsCancellationRequested);

        //    return results;
        //}
    }
}
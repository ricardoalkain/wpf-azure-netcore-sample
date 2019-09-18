using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using TTMS.Common.Abstractions;
using TTMS.Common.Models;
using TTMS.Data.Extensions;

namespace TTMS.Data.Azure
{
    public class TravelerTableWriter : BaseAzureTableProvider<Guid, Entities.Traveler>, ITravelerWriter
    {
        public TravelerTableWriter(string connectionString) : base(nameof(Traveler), connectionString)
        {
        }

        public async Task<Traveler> CreateAsync(Traveler traveler)
        {
            var entity = traveler.ToEntity();
            var operation = TableOperation.Insert(entity);
            await table.ExecuteAsync(operation);
            return traveler;
        }

        public async Task DeleteAsync(Guid id)
        {
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
            var entity = traveler.ToEntity();
            var operation = TableOperation.Replace(entity);
            await table.ExecuteAsync(operation);
        }
    }
}

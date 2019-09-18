using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using TTMS.Common.Abstractions;
using TTMS.Common.Enums;
using TTMS.Common.Models;
using TTMS.Data.Extensions;

namespace TTMS.Data.Azure
{
    public class TravelerTableReader : BaseAzureTableProvider<Guid, Entities.Traveler>, ITravelerReader
    {
        public TravelerTableReader(string connectionString) : base(nameof(Traveler), connectionString)
        {
        }

        public async Task<IEnumerable<Traveler>> GetAllAsync()
        {
            var result = await ExecuteQueryAsync();
            return result?.ToModel();
        }

        public async Task<Traveler> GetByIdAsync(Guid id)
        {
            var query = TableQuery.GenerateFilterCondition(
                                    nameof(Entities.Traveler.RowKey),
                                    QueryComparisons.Equal,
                                    id.ToString());

            var result = await ExecuteQueryAsync(query);
            return result.FirstOrDefault()?.ToModel();
        }

        public async Task<IEnumerable<Traveler>> GetByTypeAsync(TravelerType travelerType)
        {
            var query = TableQuery.GenerateFilterCondition(
                                    nameof(Entities.Traveler.PartitionKey),
                                    QueryComparisons.Equal,
                                    travelerType.ToString());

            var result = await ExecuteQueryAsync(query);
            return result?.ToModel();
        }
    }
}

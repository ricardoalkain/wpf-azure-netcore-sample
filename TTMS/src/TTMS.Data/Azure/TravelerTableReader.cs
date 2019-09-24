using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using TTMS.Common.Abstractions;
using TTMS.Common.Enums;
using TTMS.Common.Models;
using TTMS.Data.Extensions;

namespace TTMS.Data.Azure
{
    public class TravelerTableReader : BaseAzureTableProvider<Guid, Entities.Traveler>, ITravelerReader
    {
        public TravelerTableReader(ILogger<TravelerTableReader> logger, IConfiguration configuration)
            : base(logger, nameof(Traveler), configuration)
        {
        }

        public async Task<IEnumerable<Traveler>> GetAllAsync()
        {
            logger.LogDebug("{Method}", nameof(GetAllAsync));

            var result = await ExecuteQueryAsync();
            return result?.ToModel();
        }

        public async Task<Traveler> GetByIdAsync(Guid id)
        {
            logger.LogDebug("{Method} => {id}", nameof(GetByIdAsync), id);

            var query = TableQuery.GenerateFilterCondition(
                                    nameof(Entities.Traveler.RowKey),
                                    QueryComparisons.Equal,
                                    id.ToString());

            var result = await ExecuteQueryAsync(query);
            return result.FirstOrDefault()?.ToModel();
        }

        public async Task<IEnumerable<Traveler>> GetByTypeAsync(TravelerType travelerType)
        {
            logger.LogDebug("{Method} => {type}", nameof(GetByTypeAsync), travelerType);

            var query = TableQuery.GenerateFilterCondition(
                                    nameof(Entities.Traveler.PartitionKey),
                                    QueryComparisons.Equal,
                                    travelerType.ToString());

            var result = await ExecuteQueryAsync(query);
            return result?.ToModel();
        }
    }
}

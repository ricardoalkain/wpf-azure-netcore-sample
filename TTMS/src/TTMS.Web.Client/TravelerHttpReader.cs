using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TTMS.Common.Abstractions;
using TTMS.Common.Enums;
using TTMS.Common.Models;

namespace TTMS.Web.Client
{
    public class TravelerHttpReader : BaseTravelerHttpClient, ITravelerReader
    {
        public TravelerHttpReader(ILogger logger, IConfiguration configuration) : base(logger, configuration)
        {
        }

        public TravelerHttpReader(ILogger logger, string apiUrl) : base(logger, apiUrl)
        {
        }

        public async Task<IEnumerable<Traveler>> GetAllAsync()
        {
            logger.LogDebug("{Method}", nameof(GetAllAsync));

            return await retryPolicy.ExecuteAsync(async () =>
            {
                var response = await httpclient.GetAsync($"{defaultEndPoint}?loadPictures=true").ConfigureAwait(false);
                return await response.ReadAsync<IEnumerable<Traveler>>().ConfigureAwait(false);
            });
        }

        public async Task<Traveler> GetByIdAsync(Guid id)
        {
            logger.LogDebug("{Method} => {id}", nameof(GetByIdAsync), id);
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var response = await httpclient.GetAsync($"{defaultEndPoint}/{id}?loadPicture=true").ConfigureAwait(false);
                return await response.ReadAsync<Traveler>().ConfigureAwait(false);
            });
        }

        public async Task<IEnumerable<Traveler>> GetByTypeAsync(TravelerType travelerType)
        {
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var response = await httpclient.GetAsync($"{defaultEndPoint}/type/{travelerType}?loadPictures=true").ConfigureAwait(false);
                return await response.ReadAsync<IEnumerable<Traveler>>().ConfigureAwait(false);
            });
        }
    }
}

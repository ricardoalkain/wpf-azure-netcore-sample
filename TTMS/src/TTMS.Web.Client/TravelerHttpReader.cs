using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using TTMS.Common.Abstractions;
using TTMS.Common.Models;

namespace TTMS.Web.Client
{
    public class TravelerHttpReader : ITravelerReader
    {
        private const string defaultEndPoint = "beta/traveler";

        private readonly HttpClient httpclient;
        private readonly AsyncRetryPolicy retryPolicy;
        private readonly ILogger logger;

        public TravelerHttpReader(ILogger logger, string apiUrl)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (string.IsNullOrEmpty(apiUrl))
            {
                throw new ArgumentNullException(nameof(apiUrl));
            }

            httpclient = new HttpClientFactory().CreateClient(apiUrl);

            retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(10),
                onRetry: (exception, calculareDuration) =>
                {
                    logger.LogError(exception, "ERROR reading from API");
                });
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
    }
}

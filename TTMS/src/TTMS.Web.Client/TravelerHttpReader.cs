using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using TTMS.Common.Abstractions;
using TTMS.Common.Enums;
using TTMS.Common.Models;

namespace TTMS.Web.Client
{
    public class TravelerHttpReader : ITravelerReader
    {
        private const string defaultEndPoint = "beta/traveler";

        private readonly HttpClient httpclient;
        private readonly AsyncRetryPolicy retryPolicy;

        public TravelerHttpReader(string apiUrl)
        {
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
                    Console.WriteLine($"ERROR: {nameof(TravelerHttpReader)} => {exception.Message}");
                });
        }

        public async Task<IEnumerable<Traveler>> GetAllAsync()
        {
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var response = await httpclient.GetAsync($"{defaultEndPoint}?loadPictures=true").ConfigureAwait(false);
                return await response.ReadAsync<IEnumerable<Traveler>>().ConfigureAwait(false);
            });
        }

        public async Task<Traveler> GetByIdAsync(Guid id)
        {
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
                var response = await httpclient.GetAsync($"{defaultEndPoint}?type={travelerType}&loadPictures=true").ConfigureAwait(false);
                return await response.ReadAsync<IEnumerable<Traveler>>().ConfigureAwait(false);
            });
        }
    }
}

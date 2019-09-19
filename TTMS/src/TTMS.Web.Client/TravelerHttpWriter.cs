using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using TTMS.Common.Abstractions;
using TTMS.Common.DTO;
using TTMS.Common.Models;

namespace TTMS.Web.Client
{
    public class TravelerHttpWriter : ITravelerWriter
    {
        private const string defaultEndPoint = "beta/traveler";
        private const string defaultMediaType = "application/json";

        private readonly HttpClient httpclient;
        private readonly AsyncRetryPolicy retryPolicy;
        private readonly ILogger logger;

        public TravelerHttpWriter(ILogger logger, string apiUrl)
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
                onRetry: (exception, duration) =>
                {
                    logger.LogError(exception, "ERROR: {Message}", exception.Message);
                });
        }

        public async Task DeleteAsync(Guid id)
        {
            logger.LogDebug("{Method} => {id}", nameof(DeleteAsync), id);

            await retryPolicy.ExecuteAsync(async () =>
            {
                var response = await httpclient.DeleteAsync($"{defaultEndPoint}/{id}").ConfigureAwait(false);
                response.CheckResult();
            });
        }

        public async Task<Traveler> CreateAsync(Traveler traveler)
        {
            logger.LogDebug("{Method} => {@Traveler}", nameof(CreateAsync), traveler);

            var request = JsonConvert.SerializeObject(traveler.CreateRequest());

            using (var content = new StringContent(request, Encoding.UTF8, defaultMediaType))
            {
                return await retryPolicy.ExecuteAsync(async () =>
                {
                    var response = await httpclient.PostAsync(defaultEndPoint, content).ConfigureAwait(false);
                    return await response.ReadAsync<Traveler>();
                });
            }
        }

        public async Task UpdateAsync(Traveler traveler)
        {
            logger.LogDebug("{Method} => {@Traveler}", nameof(UpdateAsync), traveler);

            var request = JsonConvert.SerializeObject(traveler.CreateRequest());

            using (var content = new StringContent(request, Encoding.UTF8, defaultMediaType))
            {
                await retryPolicy.ExecuteAsync(async () =>
                {
                    var response = await httpclient.PutAsync($"{defaultEndPoint}/{traveler.Id}", content).ConfigureAwait(false);
                    response.CheckResult();
                });
            }
        }
    }
}

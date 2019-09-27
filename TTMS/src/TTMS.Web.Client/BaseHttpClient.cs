using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace TTMS.Web.Client
{
    public abstract class BaseHttpClient
    {
        protected readonly ILogger logger;
        protected readonly HttpClient httpclient;
        protected readonly AsyncRetryPolicy retryPolicy;
        protected readonly string ApiUrl;

        public BaseHttpClient(ILogger logger, IConfiguration configuration) : this(logger, configuration["ApiUrl"])
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
        }

        public BaseHttpClient(ILogger logger, string apiUrl)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (string.IsNullOrEmpty(apiUrl))
            {
                throw new ArgumentNullException(nameof(ApiUrl));
            }

            ApiUrl = apiUrl;
            httpclient = new HttpClientFactory().CreateClient(ApiUrl);

            retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(3),
                onRetry: (exception, duration) =>
                {
                    logger.LogError(exception, "{ClientClass} request failed: {Message}", this.GetType().Name, exception.Message);
                });
        }
    }
}

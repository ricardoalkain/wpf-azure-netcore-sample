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

        public BaseHttpClient(ILogger logger, IConfiguration configuration)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            ApiUrl = configuration["ApiUrl"];

            if (string.IsNullOrEmpty(ApiUrl))
            {
                throw new ArgumentNullException(nameof(ApiUrl));
            }

            httpclient = new HttpClientFactory().CreateClient(ApiUrl);

            retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(10),
                onRetry: (exception, duration) =>
                {
                    logger.LogError(exception, "{Message}", exception.Message);
                });
        }
    }
}

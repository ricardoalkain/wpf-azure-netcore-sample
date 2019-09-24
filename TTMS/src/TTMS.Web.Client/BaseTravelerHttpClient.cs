using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace TTMS.Web.Client
{
    public abstract class BaseTravelerHttpClient : BaseHttpClient
    {
        protected const string defaultEndPoint = "beta/travelers";
        protected const string defaultMediaType = "application/json";

        public BaseTravelerHttpClient(ILogger logger, IConfiguration configuration) : base(logger, configuration)
        {
        }
    }
}

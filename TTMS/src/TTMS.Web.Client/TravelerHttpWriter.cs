using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TTMS.Common.Abstractions;
using TTMS.Common.DTO.Extensions;
using TTMS.Common.Models;

namespace TTMS.Web.Client
{
    public class TravelerHttpWriter : BaseTravelerHttpClient, ITravelerWriter
    {
        public TravelerHttpWriter(ILogger logger, IConfiguration configuration) : base(logger, configuration)
        {
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

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using TTMS.Common.Abstractions;
using TTMS.Common.Models;

namespace TTMS.UI.Services
{
    public class TravelerHttpService : ITravelerService
    {
        private readonly string defaultEndPoint = "traveler";
        private readonly string defaultMimeType = "application/json";

        private readonly string baseUrl;
        private readonly HttpClient httpclient;

        public TravelerHttpService(string apiUrl)
        {
            baseUrl = apiUrl ?? throw new ArgumentNullException(nameof(apiUrl));

            httpclient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            httpclient.DefaultRequestHeaders.Accept.Clear();
            httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(defaultMimeType));
        }

        public async Task<Traveler> CreateAsync(Traveler traveler)
        {
            var content = new StringContent(JsonConvert.SerializeObject(traveler), Encoding.UTF8, defaultMimeType);

            var response = await httpclient.PutAsync(defaultEndPoint, content).ConfigureAwait(false);
            return await ProcessHttpResponse<Traveler>(response);
        }

        public async Task DeleteAsync(Guid key)
        {
            var response = await httpclient.DeleteAsync($"{defaultEndPoint}/{key}").ConfigureAwait(false);
            ProcessHttpResponse(response);
        }

        public async Task<IEnumerable<Traveler>> GetAllAsync()
        {
            var response = await httpclient.GetAsync(defaultEndPoint).ConfigureAwait(false);
            return await ProcessHttpResponse<IEnumerable<Traveler>>(response).ConfigureAwait(false);
        }

        public async Task<Traveler> GetByIdAsync(Guid key)
        {
            var response = await httpclient.GetAsync($"{defaultEndPoint}/{key}").ConfigureAwait(false);
            return await ProcessHttpResponse<Traveler>(response).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Traveler traveler)
        {
            var content = new StringContent(JsonConvert.SerializeObject(traveler), Encoding.UTF8, defaultMimeType);

            var response = await httpclient.PutAsync($"{defaultEndPoint}/{traveler.Id}", content).ConfigureAwait(false);
            ProcessHttpResponse(response);
        }

        private async Task<T> ProcessHttpResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }

            throw new HttpException((int)response.StatusCode, response.ReasonPhrase);
        }

        private void ProcessHttpResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpException((int)response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}

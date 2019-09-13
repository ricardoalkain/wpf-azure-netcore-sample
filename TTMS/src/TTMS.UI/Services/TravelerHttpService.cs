﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using TTMS.Common.Abstractions;
using TTMS.Common.Models;
using Polly;
using Polly.Retry;

namespace TTMS.UI.Services
{
    public class TravelerHttpService : ITravelerService
    {
        private readonly string defaultEndPoint = "beta/traveler";
        private readonly string defaultMimeType = "application/json";

        private readonly string baseUrl;
        private readonly HttpClient httpclient;
        private readonly RetryPolicy retryPolicy;

        public TravelerHttpService(string apiUrl)
        {
            baseUrl = apiUrl ?? throw new ArgumentNullException(nameof(apiUrl));

            httpclient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            httpclient.DefaultRequestHeaders.Accept.Clear();
            httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(defaultMimeType));

            retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(10),
                onRetry: (exception, calculareDuration) =>
                {
                    Console.WriteLine($"ERROR: {nameof(TravelerHttpService)} => {exception.Message}");
                });
        }

        public async Task<Traveler> CreateAsync(Traveler traveler)
        {
            var content = new StringContent(JsonConvert.SerializeObject(traveler), Encoding.UTF8, defaultMimeType);

            return await retryPolicy.ExecuteAsync(async () =>
            {
                var response = await httpclient.PutAsync(defaultEndPoint, content).ConfigureAwait(false);
                return await ProcessHttpResponse<Traveler>(response);
            });
        }

        public async Task DeleteAsync(Guid key)
        {
            await retryPolicy.ExecuteAsync(async () =>
            {
                var response = await httpclient.DeleteAsync($"{defaultEndPoint}/{key}").ConfigureAwait(false);
                ProcessHttpResponse(response);
            });
        }

        public async Task<IEnumerable<Traveler>> GetAllAsync()
        {
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var response = await httpclient.GetAsync($"{defaultEndPoint}?loadPictures=true").ConfigureAwait(false);
                return await ProcessHttpResponse<IEnumerable<Traveler>>(response).ConfigureAwait(false);
            });
        }

        public async Task<Traveler> GetByIdAsync(Guid id)
        {
            return await retryPolicy.ExecuteAsync(async () =>
            {
                var response = await httpclient.GetAsync($"{defaultEndPoint}/{id}?loadPicture=true").ConfigureAwait(false);
                return await ProcessHttpResponse<Traveler>(response).ConfigureAwait(false);
            });
        }

        public async Task UpdateAsync(Traveler traveler)
        {
            await retryPolicy.ExecuteAsync(async () =>
            {
                var content = new StringContent(JsonConvert.SerializeObject(traveler), Encoding.UTF8, defaultMimeType);

                var response = await httpclient.PutAsync($"{defaultEndPoint}/{traveler.Id}", content).ConfigureAwait(false);
                ProcessHttpResponse(response);
            });
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

using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TTMS.UI.Helpers
{
    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string baseUrl, string mediaType)
        {
            var httpclient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            httpclient.DefaultRequestHeaders.Accept.Clear();
            httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            return httpclient;
        }
    }
}

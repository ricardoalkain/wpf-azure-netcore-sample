using System;
using System.Net.Http;
using System.Net.Http.Headers;
using TTMS.Web.Client.Abstractions;

namespace TTMS.Web.Client
{
    public class HttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string baseUrl)
        {
            var httpclient = new HttpClient { BaseAddress = new Uri(baseUrl) };
            httpclient.DefaultRequestHeaders.Accept.Clear();
            httpclient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpclient;
        }
    }
}

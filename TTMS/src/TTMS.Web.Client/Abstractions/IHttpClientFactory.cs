using System.Net.Http;

namespace TTMS.Web.Client.Abstractions
{
    public interface IHttpClientFactory
    {
        HttpClient CreateClient(string baseUrl);
    }
}
using System.Net.Http;

namespace TTMS.UI.Helpers
{
    public interface IHttpClientFactory
    {
        HttpClient CreateClient(string baseUrl, string mediaType);
    }
}
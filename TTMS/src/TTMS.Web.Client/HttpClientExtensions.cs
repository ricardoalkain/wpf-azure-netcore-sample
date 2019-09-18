using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TTMS.Web.Client
{
    public static class HttpClientExtensions
    {
        public static async Task<T> ReadAsync<T>(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }

            throw new HttpRequestException($"ERROR {(int)response.StatusCode}: {response.ReasonPhrase}");
        }

        public static void CheckResult(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"ERROR {(int)response.StatusCode}: {response.ReasonPhrase}");
            }
        }
    }
}

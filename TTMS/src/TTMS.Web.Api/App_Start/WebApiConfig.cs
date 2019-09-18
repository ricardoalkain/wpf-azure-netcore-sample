using System.Web.Http;
using Newtonsoft.Json;

namespace TTMS.Web.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings =
                 new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

            // Web API configuration and services
            UnityConfig.RegisterComponents();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "beta/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}

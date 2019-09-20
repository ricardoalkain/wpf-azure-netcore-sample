using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TTMS.Web.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SerializerSettings =
                 new JsonSerializerSettings
                 {
                     NullValueHandling = NullValueHandling.Ignore
                 };

            // Show Enums as Strings and not as Integers
            config.Formatters.JsonFormatter.SerializerSettings
                .Converters.Add(new StringEnumConverter());

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

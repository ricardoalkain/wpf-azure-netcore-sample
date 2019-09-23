using System.Configuration;
using System.Web.Http;
using TTMS.Common.Abstractions;
using TTMS.Common.Logging;
using TTMS.Data.Azure;
using TTMS.Web.Api.Services;
using Unity;
using Unity.Injection;
using Unity.WebApi;
using MEL = Microsoft.Extensions.Logging;

namespace TTMS.Web.Api
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            var dbConnectionStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            var logLevel = ConfigurationManager.AppSettings["LogLevel"];
            var logFile = ConfigurationManager.AppSettings["LogFile"];
            var instrumentationKey = ConfigurationManager.AppSettings["InstrumentationKey"];

            container.RegisterLog("TTMS.API", logLevel, instrumentationKey, logFile);

            container.RegisterType<ITravelerReader, TravelerTableReader>(new InjectionConstructor(typeof(MEL.ILogger), dbConnectionStr));
            container.RegisterType<ITravelerWriter, TravelerTableWriter>(new InjectionConstructor(typeof(MEL.ILogger), dbConnectionStr));
            container.RegisterType<ITravelerDbService, TravelerDbService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
using System;
using System.Configuration;
using System.Web.Http;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using TTMS.Data.Repositories;
using TTMS.Web.Api.Services;
using Unity;
using Unity.Injection;
using Unity.WebApi;

namespace TTMS.Web.Api
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            var dbConnectionStr = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

            RegisterLogger(container);

            container.RegisterType<ITravelerRepository, TravelerSqlRepository>(
                new InjectionConstructor(typeof(Microsoft.Extensions.Logging.ILogger), dbConnectionStr));
            container.RegisterType<ITravelerService, TravelerApiService>();


            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }

        private static void RegisterLogger(IUnityContainer container)
        {
            var logFile = ConfigurationManager.AppSettings["LogFile"];
            if (!Enum.TryParse(ConfigurationManager.AppSettings["LogLevel"], out LogEventLevel logLevel))
            {
                logLevel = LogEventLevel.Debug;
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(logLevel)
                .WriteTo.Console()
                .WriteTo.RollingFile(logFile)
                .CreateLogger();

            var ilogger = new SerilogLoggerProvider(Log.Logger).CreateLogger("TTMS");

            container.RegisterInstance(ilogger);
        }
    }
}
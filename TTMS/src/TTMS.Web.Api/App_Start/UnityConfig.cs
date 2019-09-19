using System;
using System.Configuration;
using System.Web.Http;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using TTMS.Common.Abstractions;
using TTMS.Data.Sql;
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

            container.RegisterType<ITravelerReader, TravelerSqlReader>(new InjectionConstructor(dbConnectionStr));
            container.RegisterType<ITravelerWriter, TravelerSqlWriter>(new InjectionConstructor(dbConnectionStr));
            container.RegisterType<ITravelerDbService, TravelerDbService>();

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

            var ilogger = new SerilogLoggerProvider(Log.Logger).CreateLogger("TTMS.Api");

            container.RegisterInstance(ilogger);
        }
    }
}
using System;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using TTMS.Data.Repositories;
using TTMS.Data.Services;
using TTMS.UI.Properties;
using Unity;
using Unity.Injection;

namespace TTMS.UI.Helpers
{
    static class DependencyManager
    {
        static DependencyManager()
        {
            Container = new UnityContainer();

            RegisterLogger();

            Container
                .RegisterSingleton<ITravelerRepository, TravelerFileRepository>(new InjectionConstructor(typeof(Microsoft.Extensions.Logging.ILogger), Settings.Default.DataSource))
                .RegisterSingleton<ITravelerService, TravelerService>();
        }

        public static IUnityContainer Container { get; }

        private static void RegisterLogger()
        {
            var logFolder = Settings.Default.LogFile;
            if (!Enum.TryParse(Settings.Default.LogLevel, out LogEventLevel logLevel))
            {
                logLevel = LogEventLevel.Debug;
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(logLevel)
                .WriteTo.Console()
                .WriteTo.RollingFile(logFolder)
                .CreateLogger();

            var ilogger = new SerilogLoggerProvider(Log.Logger).CreateLogger("TTMS");

            Container.RegisterInstance(ilogger);
        }
    }
}

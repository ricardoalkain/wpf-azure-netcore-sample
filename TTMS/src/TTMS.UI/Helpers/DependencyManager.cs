using System;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using TTMS.UI.Properties;
using TTMS.UI.Services;
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
            Container.RegisterType<ITravelerService, TravelerService>(
                new InjectionConstructor(
                    Settings.Default.ApiUrl
                ));

            //Container.RegisterInstance<ITravelerService>(new TravelerHttpService(Properties.Settings.Default.ApiUrl));
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

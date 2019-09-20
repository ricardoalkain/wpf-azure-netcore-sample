using System;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Unity;

namespace TTMS.Common.Logging
{
    public static class UnityExtensions
    {
        public static IUnityContainer RegisterSerilog(this IUnityContainer container, string loggerContext, string logLevel, string logFileName = null)
        {
            if (!Enum.TryParse(logLevel, out LogEventLevel logEventLevel))
            {
                logEventLevel = LogEventLevel.Debug;
            }

            return RegisterSerilog(container, loggerContext, logEventLevel, logFileName);
        }

        public static IUnityContainer RegisterSerilog(this IUnityContainer container, string loggerContext, LogEventLevel logLevel, string logFileName = null)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Is(logLevel)
                .WriteTo.Console();

            if (!string.IsNullOrEmpty(logFileName))
            {
                loggerConfiguration.WriteTo.RollingFile(logFileName);
            }

            Log.Logger = loggerConfiguration.CreateLogger();

            var ilogger = new SerilogLoggerProvider(Log.Logger).CreateLogger(loggerContext);

            container.RegisterInstance(ilogger);

            return container;
        }
    }
}

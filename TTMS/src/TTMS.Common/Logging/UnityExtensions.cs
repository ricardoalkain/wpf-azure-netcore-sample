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
            if (!Enum.TryParse(logLevel, out Microsoft.Extensions.Logging.LogLevel logEventLevel))
            {
                logEventLevel = Microsoft.Extensions.Logging.LogLevel.Debug;
            }

            return RegisterSerilog(container, loggerContext, logEventLevel, logFileName);
        }

        public static IUnityContainer RegisterSerilog(this IUnityContainer container, string loggerContext, Microsoft.Extensions.Logging.LogLevel logLevel, string logFileName = null)
        {
            var logEventLevel = LogEventLevel.Debug;

            switch (logLevel)
            {
                case Microsoft.Extensions.Logging.LogLevel.Trace:
                    logEventLevel = LogEventLevel.Verbose;
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Debug:
                    logEventLevel = LogEventLevel.Debug;
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Information:
                    logEventLevel = LogEventLevel.Information;
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Warning:
                    logEventLevel = LogEventLevel.Warning;
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Error:
                    logEventLevel = LogEventLevel.Error;
                    break;
                case Microsoft.Extensions.Logging.LogLevel.Critical:
                    logEventLevel = LogEventLevel.Fatal;
                    break;
                case Microsoft.Extensions.Logging.LogLevel.None:
                    logEventLevel = LogEventLevel.Error;
                    break;
            }

            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Is(logEventLevel)
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

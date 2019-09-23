using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;

namespace TTMS.Common.Logging
{
    public class AppInsLogger : Microsoft.Extensions.Logging.ILogger, IDisposable
    {
        private readonly string instrumentationKey;
        private readonly TelemetryClient telemetryClient;
        private readonly Microsoft.Extensions.Logging.ILogger logger;

        public AppInsLogger(string logContext, LogLevel logLevel, string instrumentationKey = null, string logFileName = null)
        {
            this.instrumentationKey = instrumentationKey ?? throw new ArgumentNullException(nameof(instrumentationKey));

            if (!string.IsNullOrEmpty(instrumentationKey))
            {
                var telemetryConfig = TelemetryConfiguration.CreateDefault();
                telemetryConfig.InstrumentationKey = instrumentationKey;
                telemetryConfig.TelemetryInitializers.Add(new HttpDependenciesParsingTelemetryInitializer());

                telemetryClient = new TelemetryClient(telemetryConfig)
                {
                    InstrumentationKey = instrumentationKey
                };
            }

            if (!string.IsNullOrEmpty(logFileName))
            {
                var logEventLevel = LogEventLevel.Debug;

                switch (logLevel)
                {
                    case LogLevel.Trace:
                        logEventLevel = LogEventLevel.Verbose;
                        break;
                    case LogLevel.Debug:
                        logEventLevel = LogEventLevel.Debug;
                        break;
                    case LogLevel.Information:
                        logEventLevel = LogEventLevel.Information;
                        break;
                    case LogLevel.Warning:
                        logEventLevel = LogEventLevel.Warning;
                        break;
                    case LogLevel.Error:
                        logEventLevel = LogEventLevel.Error;
                        break;
                    case LogLevel.Critical:
                        logEventLevel = LogEventLevel.Fatal;
                        break;
                    case LogLevel.None:
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

                Serilog.Log.Logger = loggerConfiguration.CreateLogger();

                logger = new SerilogLoggerProvider(Serilog.Log.Logger).CreateLogger(logContext);
            }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return logger.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            logger?.Log(logLevel, eventId, state, exception, formatter);

            //if (telemetryClient != null)
            //{
            //}
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    telemetryClient?.Flush();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}

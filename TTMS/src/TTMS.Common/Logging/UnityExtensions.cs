using System;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Unity;

namespace TTMS.Common.Logging
{
    public static class UnityExtensions
    {
        /// <summary>
        /// Register an <see cref="Microsoft.Extensions.Logging.ILogger"/> instance capable of log data to console,
        /// a local file and Azure Application Insights at same time.
        /// </summary>
        /// <param name="container">Unity container</param>
        /// <param name="loggerContext">Name attributed to this log context</param>
        /// <param name="logLevel">Minimum log level</param>
        /// <param name="instrumentationKey">App Insights instrumentation key (for telemetry)</param>
        /// <param name="logFileName">Name of the local file to write the logs to</param>
        public static IUnityContainer RegisterLog(this IUnityContainer container, string loggerContext, string logLevel, string instrumentationKey, string logFileName = null)
        {
            if (!Enum.TryParse(logLevel, out Microsoft.Extensions.Logging.LogLevel logEventLevel))
            {
                logEventLevel = Microsoft.Extensions.Logging.LogLevel.Debug;
            }

            var telemetryConfig = TelemetryConfiguration.CreateDefault();
            telemetryConfig.InstrumentationKey = instrumentationKey;
            telemetryConfig.TelemetryInitializers.Add(new HttpDependenciesParsingTelemetryInitializer());


            var telemetryClient = new TelemetryClient(telemetryConfig)
            {
                InstrumentationKey = instrumentationKey
            };

            var dependencyModule = new DependencyTrackingTelemetryModule();

            // prevent Correlation Id to be sent to certain endpoints. You may add other domains as needed.
            dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("core.windows.net");
            dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("core.chinacloudapi.cn");
            dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("core.cloudapi.de");
            dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("core.usgovcloudapi.net");
            dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("localhost");
            dependencyModule.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("127.0.0.1");
            dependencyModule.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
            dependencyModule.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.EventHubs");
            dependencyModule.Initialize(telemetryConfig);

            Microsoft.Extensions.Logging.ILogger appInsLogger = new AppInsLogger(loggerContext, logEventLevel, telemetryClient, logFileName);

            container.RegisterInstance(telemetryClient);
            container.RegisterInstance(appInsLogger);

            return container;
        }
    }
}

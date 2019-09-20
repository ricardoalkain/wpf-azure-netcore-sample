using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Unity;
using Unity.Injection;

namespace TTMS.Common.Insights
{
    public static class UnityExtensions
    {
        public static IUnityContainer RegisterTelemetry(this IUnityContainer container, string instrumentationKey)
        {
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

            container.RegisterInstance(telemetryClient);

            return container;
        }
    }
}

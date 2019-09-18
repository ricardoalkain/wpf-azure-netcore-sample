using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.ServiceBus;
using TTMS.Messaging.Config;

namespace TTMS.Messaging.Consumers
{
    public abstract class BaseAzureConsumer : IMessageConsumer
    {
        private readonly TelemetryClient telemetryClient;
        private readonly DependencyTrackingTelemetryModule dependencyTrackingModule;
        private readonly QueueClient queueClient;

        public BaseAzureConsumer(MessagingConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var telemetryConfig = TelemetryConfiguration.CreateDefault();
            telemetryConfig.InstrumentationKey = config.InstrumentationKey;
            telemetryConfig.TelemetryInitializers.Add(new HttpDependenciesParsingTelemetryInitializer());

            telemetryClient = new TelemetryClient(telemetryConfig)
            {
                InstrumentationKey = config.InstrumentationKey
            };

            dependencyTrackingModule = InitializeDependencyTracking(telemetryConfig);

            queueClient = new QueueClient(config.ServerConnection, config.IncomingQueue);
        }

        public void StartListening()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            //queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        public abstract Task ProcessMessageAsync(string message);

        public void Dispose()
        {
            queueClient.CloseAsync().Wait();
            telemetryClient.Flush();
            dependencyTrackingModule.Dispose();
        }






        private async Task ProcessMessagesAsync(Microsoft.Azure.ServiceBus.Message message, CancellationToken token)
        {
            string stringMessage = Encoding.UTF8.GetString(message.Body);
            await ProcessMessageAsync(stringMessage);
            telemetryClient.TrackEvent("New TTMS message received => " + stringMessage);

            if (!token.IsCancellationRequested)
            {
                await queueClient.CompleteAsync(message.SystemProperties.LockToken);
            }
        }


        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
        {
            Console.WriteLine($"{this.GetType().Name} encountered an exception {args.Exception}.");
            telemetryClient.TrackException(args.Exception);
            var context = args.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($" - Endpoint: {context.Endpoint}");
            Console.WriteLine($" - Path:     {context.EntityPath}");
            Console.WriteLine($" - Action:   {context.Action}");

            return Task.CompletedTask;
        }

        private DependencyTrackingTelemetryModule InitializeDependencyTracking(TelemetryConfiguration configuration)
        {
            var module = new DependencyTrackingTelemetryModule();

            // prevent Correlation Id to be sent to certain endpoints. You may add other domains as needed.
            module.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("core.windows.net");
            module.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("core.chinacloudapi.cn");
            module.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("core.cloudapi.de");
            module.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("core.usgovcloudapi.net");
            module.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("localhost");
            module.ExcludeComponentCorrelationHttpHeadersOnDomains.Add("127.0.0.1");

            module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.ServiceBus");
            module.IncludeDiagnosticSourceActivities.Add("Microsoft.Azure.EventHubs");

            module.Initialize(configuration);

            return module;
        }

    }
}

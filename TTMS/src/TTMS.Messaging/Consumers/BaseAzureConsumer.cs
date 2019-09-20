using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using TTMS.Messaging.Config;

namespace TTMS.Messaging.Consumers
{
    public abstract class BaseAzureConsumer : IMessageConsumer
    {
        protected readonly TelemetryClient telemetryClient;
        protected readonly QueueClient queueClient;
        protected readonly ILogger logger;
        protected readonly DependencyTrackingTelemetryModule dependencyTrackingModule;

        public BaseAzureConsumer(
            ILogger logger,
            TelemetryClient telemetryClient,
            MessagingConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));

            this.logger.LogInformation("Initializing Service Bus client (queue {queue})...", config.IncomingQueue);
            queueClient = new QueueClient(config.ServerConnection, config.IncomingQueue);
        }

        public void StartListening()
        {
            logger.LogInformation("Starting Service Bus consumer...");
            telemetryClient.TrackEvent($"Consumer {this.GetType().FullName} started");

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        public abstract Task ProcessMessageAsync(string message);

        public void Dispose()
        {
            logger.LogInformation("Stopping Service Bus consumer...");
            telemetryClient.TrackEvent($"Consumer {this.GetType().FullName} stopped");
            telemetryClient.Flush();

            queueClient.CloseAsync().Wait();
        }






        private async Task ProcessMessagesAsync(Microsoft.Azure.ServiceBus.Message message, CancellationToken token)
        {
            logger.LogInformation("Message received: {info}", message.MessageId);

            string stringMessage = Encoding.UTF8.GetString(message.Body);
            await ProcessMessageAsync(stringMessage);

            telemetryClient.TrackEvent("New TTMS message processed");

            if (!token.IsCancellationRequested)
            {
                await queueClient.CompleteAsync(message.SystemProperties.LockToken);
            }
        }


        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
        {
            logger.LogError(args.Exception, "Service Bus consumer error!");
            telemetryClient.TrackException(args.Exception);

            return Task.CompletedTask;
        }
    }
}

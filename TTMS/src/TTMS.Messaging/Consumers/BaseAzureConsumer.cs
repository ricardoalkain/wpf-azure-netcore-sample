using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using TTMS.Messaging.Config;

namespace TTMS.Messaging.Consumers
{
    public abstract class BaseAzureConsumer : IMessageConsumer
    {
        protected readonly QueueClient queueClient;
        protected readonly ILogger logger;

        public BaseAzureConsumer(
            ILogger logger,
            MessagingConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

            this.logger.LogInformation("Initializing Service Bus client (queue {queue})...", config.IncomingQueue);
            queueClient = new QueueClient(config.ServerConnection, config.IncomingQueue);
        }

        public void StartListening()
        {
            logger.LogInformation("Starting Service Bus consumer...");

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

            queueClient.CloseAsync().Wait();
        }

        private async Task ProcessMessagesAsync(Microsoft.Azure.ServiceBus.Message message, CancellationToken token)
        {
            logger.LogInformation("Message received: {info}", message.MessageId);

            string stringMessage = Encoding.UTF8.GetString(message.Body);
            await ProcessMessageAsync(stringMessage);

            if (!token.IsCancellationRequested)
            {
                await queueClient.CompleteAsync(message.SystemProperties.LockToken);
            }
        }


        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
        {
            logger.LogError(args.Exception, "Service Bus consumer error!");

            return Task.CompletedTask;
        }
    }
}

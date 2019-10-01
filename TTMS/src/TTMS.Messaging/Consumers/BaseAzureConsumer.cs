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
        protected readonly IQueueClient queueClient;
        protected readonly ILogger logger;

        public BaseAzureConsumer(
            ILogger logger,
            IQueueClient queueClient)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.queueClient = queueClient ?? throw new ArgumentNullException(nameof(queueClient));

            this.logger.LogInformation("Initializing Service Bus client (queue {queue})...", queueClient.QueueName);
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
            logger.LogInformation("Message received [ID {messageId}]", message.MessageId);

            string stringMessage = Encoding.UTF8.GetString(message.Body);

            if (!token.IsCancellationRequested)
            {
                logger.LogInformation("Processing message [ID {messageId}]...", message.MessageId);
                await ProcessMessageAsync(stringMessage);
                logger.LogInformation("Message successfully processed [ID {messageId}]", message.MessageId);

                await queueClient.CompleteAsync(message.SystemProperties.LockToken);
                logger.LogInformation("Message committed [ID {messageId}]", message.MessageId);
            }
        }


        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
        {
            logger.LogError(args.Exception, "Service Bus consumer error!");

            return Task.CompletedTask;
        }
    }
}

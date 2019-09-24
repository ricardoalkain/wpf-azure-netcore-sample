using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TTMS.Common.Messages;
using TTMS.Messaging.Config;

namespace TTMS.Messaging.Producers
{
    public class AzureServiceBusProducer<T> : IMessageProducer<T>, IDisposable
    {
        private readonly ILogger logger;
        private readonly MessagingConfig config;
        private readonly IQueueClient queueClient;

        public AzureServiceBusProducer(
            ILogger logger,
            MessagingConfig config)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.config = config ?? throw new ArgumentNullException(nameof(config));

            this.queueClient = new QueueClient(config.ServerConnection, config.OutgoingQueue);
        }

        public async Task PublishAsync(BaseMessage<T> message)
        {
            var json = JsonConvert.SerializeObject(message);
            var payload = new Microsoft.Azure.ServiceBus.Message(Encoding.UTF8.GetBytes(json));

            await queueClient.SendAsync(payload).ConfigureAwait(false);
            logger.LogInformation($"Publish message to {config.OutgoingQueue}");
        }

        public async Task PublishAsync(MessageType messageType, T content, Guid messageKey = default)
        {
            var message = new BaseMessage<T>
            {
                Key = messageKey == default ? Guid.NewGuid() : messageKey,
                Type = messageType,
                Content = content
            };

            if (message.Key == default)
            {
                message.Key = Guid.NewGuid();
            }

            await PublishAsync(message).ConfigureAwait(false);
        }

        public void Dispose()
        {
            queueClient.CloseAsync();
        }
    }
}

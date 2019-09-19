using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TTMS.Common.Abstractions;
using TTMS.Messaging.Config;

namespace TTMS.Messaging.Consumers
{
    public class TravelerConsumer : BaseRabbitMQConsumer, IMessageConsumer
    {
        private readonly ITravelerWriter writer;
        private readonly ILogger logger;

        public TravelerConsumer(ILogger logger, MessagingConfig messagingConfig, ITravelerWriter travelerWriter) : base(logger, messagingConfig)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.writer = travelerWriter ?? throw new ArgumentNullException(nameof(travelerWriter));
        }

        public override async void ProcessMessage(string jsonMessage)
        {
            logger.LogDebug("Deserializing message...");
            var msg = JsonConvert.DeserializeObject<TravelerMessage>(jsonMessage);
            logger.LogInformation("Traveler Message received: {type}", msg.Type);


            switch (msg.Type)
            {
                case MessageType.Create:
                    await writer.CreateAsync(msg.Content).ConfigureAwait(false);
                    break;
                case MessageType.Update:
                    await writer.UpdateAsync(msg.Content).ConfigureAwait(false);
                    break;
                case MessageType.Delete:
                    await writer.DeleteAsync(msg.Content.Id).ConfigureAwait(false);
                    break;
                default:
                    var ex = new NotImplementedException($"No action implemented for messages of type {msg.Type}");
                    logger.LogError(ex, "Error routing Traveler message");
                    throw ex;
            }
        }
    }
}

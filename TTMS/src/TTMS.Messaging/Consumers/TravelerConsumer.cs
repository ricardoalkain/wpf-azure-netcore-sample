using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TTMS.Common.Abstractions;
using TTMS.Common.Messages;
using TTMS.Messaging.Config;

namespace TTMS.Messaging.Consumers
{
    public class TravelerConsumer : BaseAzureConsumer
    {
        private readonly ITravelerWriter writer;

        public TravelerConsumer(
            ILogger logger,
            MessagingConfig messagingConfig,
            ITravelerWriter travelerWriter) : base(logger, messagingConfig)
        {
            this.writer = travelerWriter ?? throw new ArgumentNullException(nameof(travelerWriter));
        }

        public override async Task ProcessMessageAsync(string jsonMessage)
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

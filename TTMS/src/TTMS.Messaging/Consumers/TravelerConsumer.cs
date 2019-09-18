using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using TTMS.Common.Abstractions;
using TTMS.Messaging.Config;

namespace TTMS.Messaging.Consumers
{
    public class TravelerConsumer : BaseRabbitMQConsumer
    {
        private readonly ITravelerWriter writer;

        public TravelerConsumer(MessagingConfig messagingConfig, ITravelerWriter travelerWriter) : base(messagingConfig)
        {
            writer = travelerWriter ?? throw new ArgumentNullException(nameof(travelerWriter));
        }

        public override async void ProcessMessage(string jsonMessage)
        {
            var msg = JsonConvert.DeserializeObject<TravelerMessage>(jsonMessage);

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
                    throw new NotImplementedException($"No action implemented for messages of type {msg.Type}");
            }
        }
    }
}

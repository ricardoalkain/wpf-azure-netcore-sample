using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TTMS.Common.Abstractions;
using TTMS.Common.Models;

namespace TTMS.Messaging.Producers
{
    public class TravelerMessageWriter : ITravelerWriter
    {
        private readonly ILogger logger;
        private readonly IMessageProducer<Traveler> producer;

        public TravelerMessageWriter(ILogger logger, IMessageProducer<Traveler> messageProducer)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.producer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
        }

        public async Task<Traveler> CreateAsync(Traveler traveler)
        {
            logger.LogDebug("Sending message for new traveler => {@traveler}", traveler);

            await Task.Run(() =>
            {
                producer.Publish(MessageType.Create, traveler);
            });

            return traveler;
        }

        public async Task DeleteAsync(Guid id)
        {
            logger.LogDebug("Sending message for detelting traveler => {id}", id);

            await Task.Run(() =>
            {
                var traveler = new Traveler { Id = id };
                producer.Publish(MessageType.Delete, traveler);
            });
        }

        public async Task UpdateAsync(Traveler traveler)
        {
            logger.LogDebug("Sending message for updating traveler => {@traveler}", traveler);

            await Task.Run(() =>
            {
                producer.Publish(MessageType.Update, traveler);
            });
        }
    }
}

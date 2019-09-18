using System;
using System.Threading.Tasks;
using TTMS.Common.Abstractions;
using TTMS.Common.Entities;

namespace TTMS.Messaging.Producers
{
    public class TravelerMessageWriter : ITravelerWriter
    {
        private readonly IMessageProducer<Traveler> producer;

        public TravelerMessageWriter(IMessageProducer<Traveler> messageProducer)
        {
            producer = messageProducer ?? throw new ArgumentNullException(nameof(messageProducer));
        }

        public async Task<Traveler> CreateAsync(Traveler traveler)
        {
            await Task.Run(() =>
            {
                producer.Publish(MessageType.Create, traveler);
            });

            return traveler;
        }

        public async Task DeleteAsync(Guid id)
        {
            await Task.Run(() =>
            {
                var traveler = new Traveler { Id = id };
                producer.Publish(MessageType.Delete, traveler);
            });
        }

        public async Task UpdateAsync(Traveler traveler)
        {
            await Task.Run(() =>
            {
                producer.Publish(MessageType.Update, traveler);
            });
        }
    }
}

using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TTMS.Common.Abstractions;
using TTMS.ConsumerService.Properties;
using TTMS.Messaging;

namespace TTMS.ConsumerService
{
    public class Listener : IListener
    {
        private readonly ITravelerService travelerService;
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly Settings appSettings;

        internal Listener(Settings appSettings, ITravelerService travelerService)
        {
            this.appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            this.travelerService = travelerService ?? throw new ArgumentNullException(nameof(travelerService));

            var rabbitConnectionFactory = new ConnectionFactory { Uri = new Uri(appSettings.MessageBusConnection) };
            this.connection = rabbitConnectionFactory.CreateConnection();
            this.channel = connection.CreateModel();
        }

        public void Start()
        {
            channel.QueueDeclare(appSettings.IncomingMessageQueue, true, false, false, null);
            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(this.channel);

            channel.BasicConsume(queue: appSettings.IncomingMessageQueue, autoAck: false, consumer: consumer);


            consumer.Received += (model, args) =>
            {
                var body = args.Body;

                string receivedMessage = null;

                try
                {
                    receivedMessage = Encoding.UTF8.GetString(body);
                    ProcessMessage(receivedMessage);
                    channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
                }
                catch
                {
                    channel.BasicReject(deliveryTag: args.DeliveryTag, false);
                }
            };
        }

        private async void ProcessMessage(string receivedMessage)
        {
            var msg = JsonConvert.DeserializeObject<TravelerMessage>(receivedMessage);

            switch (msg.Type)
            {
                case MessageType.Create:
                    await travelerService.CreateAsync(msg.Content).ConfigureAwait(false);
                    break;
                case MessageType.Update:
                    await travelerService.UpdateAsync(msg.Content).ConfigureAwait(false);
                    break;
                case MessageType.Delete:
                    await travelerService.DeleteAsync(msg.Content.Id).ConfigureAwait(false);
                    break;
                default:
                    throw new NotImplementedException($"No action implemented for messages of type {msg.Type}");
            }
        }

        public void Stop()
        {
            this.connection.Dispose();
            this.channel.Dispose();
        }
    }
}

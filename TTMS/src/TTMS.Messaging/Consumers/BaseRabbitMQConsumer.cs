using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TTMS.Common.Abstractions;
using TTMS.Messaging;
using TTMS.Messaging.Config;

namespace TTMS.Messaging.Consumers
{
    public abstract class BaseRabbitMQConsumer : IMessageConsumer
    {
        protected readonly IConnection connection;
        protected readonly IModel channel;
        protected readonly MessagingConfig config;

        public BaseRabbitMQConsumer(MessagingConfig messagingConfig)
        {
            config = messagingConfig ?? throw new ArgumentNullException(nameof(messagingConfig));

            var rabbitConnectionFactory = new ConnectionFactory { Uri = new Uri(messagingConfig.ServerConnection) };
            connection = rabbitConnectionFactory.CreateConnection();
            channel = connection.CreateModel();
        }

        public void StartListening()
        {
            channel.QueueDeclare(config.IncomingQueue, true, false, false, null);
            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(queue: config.IncomingQueue, autoAck: false, consumer: consumer);


            consumer.Received += (model, args) =>
            {
                var body = args.Body;

                string receivedMessage = null;

                try
                {
                    receivedMessage = Encoding.UTF8.GetString(body);
                    ProcessMessageAsync(receivedMessage);
                    channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
                }
                catch
                {
                    channel.BasicReject(deliveryTag: args.DeliveryTag, false);
                }
            };
        }

        public abstract Task ProcessMessageAsync(string jsonMessage);

        public void Dispose()
        {
            connection.Dispose();
            channel.Dispose();
        }
    }
}
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger logger;
        protected readonly MessagingConfig config;

        public BaseRabbitMQConsumer(ILogger logger, MessagingConfig messagingConfig)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.config = messagingConfig ?? throw new ArgumentNullException(nameof(messagingConfig));

            logger.LogInformation("Creating RabbitMQ consumer...");

            var rabbitConnectionFactory = new ConnectionFactory { Uri = new Uri(messagingConfig.ServerConnection) };
            this.connection = rabbitConnectionFactory.CreateConnection();
            this.channel = connection.CreateModel();
        }

        public void StartListening()
        {
            logger.LogInformation("Start listening RabbitMQ queue: {queue}", config.IncomingQueue);

            channel.QueueDeclare(config.IncomingQueue, true, false, false, null);
            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(queue: config.IncomingQueue, autoAck: false, consumer: consumer);


            consumer.Received += (model, args) =>
            {
                var body = args.Body;
                logger.LogDebug("Message received ({size} bytes)", body.Length);

                string receivedMessage;

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
            logger.LogInformation("Disposing RabbitMQ consumer...");

            connection.Dispose();
            channel.Dispose();
        }
    }
}
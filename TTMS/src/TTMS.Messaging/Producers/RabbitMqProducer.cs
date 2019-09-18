using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using TTMS.Messaging.Config;

namespace TTMS.Messaging.Producers
{
    public abstract class RabbitMqProducer<T> : IMessageProducer<T>
    {
        private readonly ConnectionFactory factory;
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string queue;

        public RabbitMqProducer(MessagingConfig messagingConfig)
        {
            factory = new ConnectionFactory { Uri = new Uri(messagingConfig.ServerConnection) };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            queue = messagingConfig.OutgoingQueue;
            channel.QueueDeclare(queue, true, false, false, null);
        }

        public void Publish(BaseMessage<T> message)
        {
            var payload = JsonConvert.SerializeObject(message);
            channel.BasicPublish("", queue, true, null, Encoding.UTF8.GetBytes(payload));
        }

        public void Publish(MessageType messageType, T content, Guid messagekey = default)
        {
            var message = new BaseMessage<T>
            {
                Type = messageType,
                Content = content,
                Key = messagekey == default ? Guid.NewGuid() : messagekey
            };
        }
    }
}

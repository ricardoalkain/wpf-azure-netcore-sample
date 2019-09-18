using System;
using System.Collections.Generic;
using System.Text;

namespace TTMS.Messaging.Producers
{
    public interface IMessageProducer<T>
    {
        void Publish(BaseMessage<T> message);

        void Publish(MessageType messageType, T content, Guid messageKey = default);
    }
}

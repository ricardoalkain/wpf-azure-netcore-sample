using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TTMS.Common.Messages;

namespace TTMS.Messaging.Producers
{
    public interface IMessageProducer<T>
    {
        Task PublishAsync(BaseMessage<T> message);

        Task PublishAsync(MessageType messageType, T content, Guid messageKey = default);
    }
}

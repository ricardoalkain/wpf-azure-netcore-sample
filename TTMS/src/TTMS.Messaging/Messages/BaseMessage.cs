using System;
using System.Collections.Generic;
using System.Text;

namespace TTMS.Messaging
{
    public abstract class BaseMessage<T>
    {
        public Guid Key { get; set; }

        public MessageType Type { get; set; }

        public T Content { get; set; }
    }
}

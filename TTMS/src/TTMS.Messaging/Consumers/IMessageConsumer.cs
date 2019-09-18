using System;
using System.Collections.Generic;
using System.Text;

namespace TTMS.Messaging.Consumers
{
    public interface IMessageConsumer : IDisposable
    {
        void StartListening();

        void ProcessMessage(string message);
    }
}

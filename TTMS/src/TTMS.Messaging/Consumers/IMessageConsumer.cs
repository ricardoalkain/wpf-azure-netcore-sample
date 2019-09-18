using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TTMS.Messaging.Consumers
{
    public interface IMessageConsumer : IDisposable
    {
        void StartListening();

        Task ProcessMessageAsync(string message);
    }
}

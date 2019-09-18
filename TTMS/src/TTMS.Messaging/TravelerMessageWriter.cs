using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TTMS.Common.Abstractions;
using TTMS.Common.Models;
using TTMS.Messaging.Config;

namespace TTMS.Messaging
{
    public class TravelerMessageWriter : ITravelerWriter
    {
        public TravelerMessageWriter(MessagingConfig messagingConfig)
        {

        }

        public Task<Traveler> CreateAsync(Traveler entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Traveler entity)
        {
            throw new NotImplementedException();
        }
    }
}

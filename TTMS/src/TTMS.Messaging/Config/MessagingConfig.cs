using System;
using System.Collections.Generic;
using System.Text;

namespace TTMS.Messaging.Config
{
    public class MessagingConfig
    {
        public string ServerConnection { get; set; }

        public string IncomingQueue { get; set; }

        public string OutgoingQueue { get; set; }
    }
}

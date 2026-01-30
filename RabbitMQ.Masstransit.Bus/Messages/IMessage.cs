using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Masstransit.Bus.Messages
{
    public interface IMessage
    {
        public string Text { get; set; }
    }
}

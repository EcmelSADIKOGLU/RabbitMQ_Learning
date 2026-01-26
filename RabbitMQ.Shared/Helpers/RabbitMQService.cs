using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ.Shared.Helpers
{
    public class RabbitMQService
    {
        public static async Task<(IChannel channel, IConnection connection)> CreateChannel()
        {
            //Create Connection
            ConnectionFactory factory = new ConnectionFactory();
            factory.Uri = new("amqps://zubzgupk:gqHuAcnSTkzF5gfbSkxJB4Z_oMHHSzPX@stingray.rmq.cloudamqp.com/zubzgupk");

            // Activate Connection - Open Channel
            IConnection connection = await factory.CreateConnectionAsync();
            IChannel channel = await connection.CreateChannelAsync();

            return (channel,connection);
        }
    }
}

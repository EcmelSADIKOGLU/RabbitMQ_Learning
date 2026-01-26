using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;
using static RabbitMQ.Shared.Helpers.RabbitMQService;

var (channel,connection) = await CreateChannel();

using (connection) using (channel)
{

    // Declare Queue
    await channel.QueueDeclareAsync(queue: "example-queue", exclusive: false, durable: true);


    // Publish Messages
    // RabbitMQ message body is byte array
    byte[] message = Encoding.UTF8.GetBytes("Hello from RabbitMQ Example Publisher!");

    await channel.BasicPublishAsync(exchange: "", routingKey: "example-queue", body: message, mandatory: false, basicProperties: new BasicProperties
    {
        Persistent = true
    });
}

Console.Read();


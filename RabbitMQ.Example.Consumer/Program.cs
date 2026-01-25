using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//Create Connection
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://zubzgupk:gqHuAcnSTkzF5gfbSkxJB4Z_oMHHSzPX@stingray.rmq.cloudamqp.com/zubzgupk");

// Activate Connection - Open Channel
using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

// Declare Queue
// It have to be exactly the same with publisher
await channel.QueueDeclareAsync(queue: "example-queue", exclusive:false);

// Read Messages
AsyncEventingBasicConsumer consumer = new(channel);
string message = await channel.BasicConsumeAsync(queue: "example-queue", autoAck: false, consumer);
await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

consumer.ReceivedAsync += async (sender, ea) =>
{
    byte[] body = ea.Body.ToArray();
    string messageText = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Message: {messageText}");

    // Message handled
    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
};

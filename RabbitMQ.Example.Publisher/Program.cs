using RabbitMQ.Client;
using System.Text;

//Create Connection
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://zubzgupk:gqHuAcnSTkzF5gfbSkxJB4Z_oMHHSzPX@stingray.rmq.cloudamqp.com/zubzgupk");

// Activate Connection - Open Channel
using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

// Declare Queue
await channel.QueueDeclareAsync(queue: "example-queue", exclusive:false);

// Publish Messages
// RabbitMQ message body is byte array

byte[] message = Encoding.UTF8.GetBytes("Hello from RabbitMQ Example Publisher!");
await channel.BasicPublishAsync(exchange: "", routingKey: "example-queue", body: message);

Console.Read();

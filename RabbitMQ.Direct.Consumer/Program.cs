using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using static RabbitMQ.Shared.Helpers.RabbitMQService;

var (channel, connection) = await CreateChannel();

using (connection) using (channel)
{
    //Declare Exchange exactly the same with publisher
    await channel.ExchangeDeclareAsync(
       exchange: "direct-example-exchange",
       type: ExchangeType.Direct);

    // Declare Queue
    var queue = await channel.QueueDeclareAsync(exclusive:false); // Let RabbitMQ create a random queue name
    var queueName = queue.QueueName;

    // Bind Queue to Exchange with Routing Key
    await channel.QueueBindAsync(
        queue: queueName,
        exchange: "direct-example-exchange",
        routingKey: "direct-example-route");

    //Create Consumer
    AsyncEventingBasicConsumer consumer = new(channel);

    // Read Messages
    await channel.BasicConsumeAsync(
        queue: queueName,
        autoAck: true,
        consumer: consumer);

    // When Message Received trigger fonction
    consumer.ReceivedAsync += async (sender, ea) =>
    {
        var message = Encoding.UTF8.GetString(ea.Body.Span);
        Console.WriteLine($"Message Received: {message}");
    };

    Console.Read();
}



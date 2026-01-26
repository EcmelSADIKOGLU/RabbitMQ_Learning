using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using static RabbitMQ.Shared.Helpers.RabbitMQService;

var (channel, connection) = await CreateChannel();

using (connection) using (channel)
{
    //Declare Exchange
    // It must be exactly the same with publisher
    await channel.ExchangeDeclareAsync(
        exchange: "fanout-example-exchange",
        type: ExchangeType.Fanout);

    // Declare Queue
    var queue = await channel.QueueDeclareAsync(exclusive: false); // Let RabbitMQ create a random queue name
    var queueName = queue.QueueName;

    // Bind Queue to Exchange
    await channel.QueueBindAsync(
        queue: queueName,
        exchange: "fanout-example-exchange",
        routingKey: "ignored"); // Routing Key is ignored for Fanout Exchange

    //Create Consumer
    AsyncEventingBasicConsumer consumer = new(channel);
    
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
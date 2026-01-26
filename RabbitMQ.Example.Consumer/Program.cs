using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using static RabbitMQ.Shared.Helpers.RabbitMQService;

var (channel, connection) = await CreateChannel();

using (connection) using (channel)
{
    // Declare Queue
    // It have to be exactly the same with publisher
    await channel.QueueDeclareAsync(queue: "example-queue", exclusive: false);

    await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

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
}

Console.Read();



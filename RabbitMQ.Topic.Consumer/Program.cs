using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using static RabbitMQ.Shared.Helpers.RabbitMQService;

var (channel, connection) = await CreateChannel();

using (connection) using (channel)
{

    await channel.ExchangeDeclareAsync(
        exchange: "topic_logs",
        type: ExchangeType.Topic);

    QueueDeclareOk queue = await channel.QueueDeclareAsync(exclusive: false);
    string queueName = queue.QueueName;

    Console.Write("Enter topic: ");
    string topic = string.Empty;
    topic = Console.ReadLine();

    await channel.QueueBindAsync(
        queue: queueName,
        exchange: "topic-example-exchange",
        routingKey: topic);

    AsyncEventingBasicConsumer consumer = new(channel);

    await channel.BasicConsumeAsync(
        queue: queueName,
        autoAck: true,
        consumer: consumer);

    consumer.ReceivedAsync += async (sender, ea) =>
    {
        var message = Encoding.UTF8.GetString(ea.Body.Span);
        Console.WriteLine($"Message: {message}");
    };


    Console.ReadLine();
}
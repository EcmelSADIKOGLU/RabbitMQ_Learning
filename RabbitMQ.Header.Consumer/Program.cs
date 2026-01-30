using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using static RabbitMQ.Shared.Helpers.RabbitMQService;

var (channel, connection) = await CreateChannel();

using (connection) using (channel)
{

    await channel.ExchangeDeclareAsync(
        exchange: "header-example-exchange",
        type: ExchangeType.Headers);

    QueueDeclareOk queue = await channel.QueueDeclareAsync(exclusive: false);
    string queueName = queue.QueueName;


    await channel.QueueBindAsync(
        queue: queueName,
        exchange: "header-example-exchange",
        routingKey: string.Empty,

        arguments: new Dictionary<string, object?>
        {
            ["x-match"] = "all", // "any" for OR logic
            ["format"] = "pdf",
            ["shape"] = "a4"
        });

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
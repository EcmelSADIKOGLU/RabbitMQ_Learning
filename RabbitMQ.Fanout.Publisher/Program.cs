using RabbitMQ.Client;
using System.Text;
using static RabbitMQ.Shared.Helpers.RabbitMQService;

var (channel, connection) = await CreateChannel();

using (connection) using (channel)
{
    //Declare Exchange
    await channel.ExchangeDeclareAsync(
        exchange: "fanout-example-exchange",
        type: ExchangeType.Fanout);

    for (int i = 0; i < 100; i++)
    {
        await Task.Delay(200);

        string message = $"Hello Fanout Exchange! Message Number: {i}";
        var body = Encoding.UTF8.GetBytes(message);

        //Publish Message to Exchange
        await channel.BasicPublishAsync(
            exchange: "fanout-example-exchange",
            routingKey: "not important", // Routing Key is ignored for Fanout Exchange
            body: body);

        Console.WriteLine($"Message Sent: {message}");
    }

    Console.Read();
}

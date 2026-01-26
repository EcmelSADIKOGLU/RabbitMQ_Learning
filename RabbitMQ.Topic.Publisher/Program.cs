using RabbitMQ.Client;
using System.Text;
using static RabbitMQ.Shared.Helpers.RabbitMQService;

var (channel, connection) = await CreateChannel();

using (connection) using (channel)
{

    await channel.ExchangeDeclareAsync(
        exchange: "topic-example-exchange",
        type: ExchangeType.Topic);


    for (int i = 0; i < 100; i++)
    {
        string topic = string.Empty;
        Console.Write("Enter Topic : ");
        topic = Console.ReadLine();

        string message = $"Message {i} from Topic Exchange publisher";
        byte[] body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(
            exchange: "topic-example-exchange",
            routingKey: topic,
            body: body);
    }

    Console.ReadLine();
}

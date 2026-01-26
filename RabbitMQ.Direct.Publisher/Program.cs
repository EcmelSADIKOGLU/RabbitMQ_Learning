using RabbitMQ.Client;
using System.Text;
using static RabbitMQ.Shared.Helpers.RabbitMQService;


var (channel, connection) = await CreateChannel();

using (connection) using (channel)
{
    // Declare Exchange
    // Direct Exchange send message just the same routing key queues
    await channel.ExchangeDeclareAsync(
        exchange: "direct-example-exchange",
        type: ExchangeType.Direct);


    for (int i = 0; i < 100; i++)
    {
        await Task.Delay(200);
        string message = $"Hello {i} from RabbitMQ Direct Publisher!";
        byte[] body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(
            exchange: "direct-example-exchange",
            routingKey: "direct-example-route",
            body: body);


    }
    Console.Read();
}




using RabbitMQ.Client;
using System.Text;
using static RabbitMQ.Shared.Helpers.RabbitMQService;

var (channel, connection) = await CreateChannel();

using (connection) using (channel)
{

    await channel.ExchangeDeclareAsync(
        exchange: "header-example-exchange",
        type: ExchangeType.Headers);


    for (int i = 0; i < 100; i++)
    {
        await Task.Delay(200);

        string message = $"Message {i} from Header Exchange publisher";
        byte[] body = Encoding.UTF8.GetBytes(message);


        
        await channel.BasicPublishAsync(
            exchange: "header-example-exchange",
            routingKey: string.Empty,
            mandatory: false,
            basicProperties: new BasicProperties
            {
                Headers = new Dictionary<string, object?>
                {
                    ["format"] = "pdf",
                    ["shape"] = "a4"
                }
            },
            body: body);
    }

    Console.ReadLine();
}
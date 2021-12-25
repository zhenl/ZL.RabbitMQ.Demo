using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;


var factory = new ConnectionFactory()
{
    HostName = "127.0.0.1",
    UserName = "admin",
    Password = "admin",
    VirtualHost = "my_vhost"
};
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.ExchangeDeclare(exchange: "example.exchange",
type: "fanout", durable: true);

    channel.QueueDeclare(queue: "que2",
                         durable: true,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);
    channel.QueueBind(queue: "que2", exchange: "example.exchange",
routingKey: "");
    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body;
        var message = Encoding.UTF8.GetString(body.ToArray());
        Console.WriteLine("收到消息 {0}", message);
    };
    channel.BasicConsume(queue: "que2",
                         autoAck: true,
                         consumer: consumer);

    Console.WriteLine(" 按回车退出");
    Console.ReadLine();
}
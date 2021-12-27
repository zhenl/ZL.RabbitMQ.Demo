using RabbitMQ.Client;
using System.Text;


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
    channel.ExchangeDeclare("directdemo.exchange", ExchangeType.Direct, true, false, null);

    Console.WriteLine("输入需要传输的消息，输入Exit退出");
    var message = Console.ReadLine();
    while (message != "Exit")
    {
        Console.WriteLine("输入RouteKey");
        var routekey = Console.ReadLine();
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "directdemo.exchange",
                             routingKey: routekey,
                             basicProperties: null,
                             body: body);
        Console.WriteLine(" 发送消息 {0} Routekey {1}", message,routekey);
        message = Console.ReadLine();
    }
}

Console.WriteLine("按回车退出");
Console.ReadLine();
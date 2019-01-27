using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMq.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var factory = new ConnectionFactory();
            factory.HostName = "localhost";//RabbitMQ服务在本地运行
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            factory.UserName = "lwl";//用户名
            factory.Password = "Zaq123";//密码
            factory.Protocol = Protocols.DefaultProtocol;
            factory.AutomaticRecoveryEnabled = true;
            using (var connection = factory.CreateConnection())
            {


                using (var channel = connection.CreateModel())
                {

                    channel.ExchangeDeclare(exchange: "exchange-direct", type: "direct");
                    string name = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: name, exchange: "exchange-direct", routingKey: "routing-delay");

                    //回调，当consumer收到消息后会执行该函数
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(ea.RoutingKey);
                        Console.WriteLine(" [x] Received {0}", message);
                    };

                    //Console.WriteLine("name:" + name);
                    //消费队列"hello"中的消息
                    channel.BasicConsume(queue: name,
                                         noAck: true,
                                         consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
                Console.ReadKey();

            }


        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitMqTest
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
                
                    while (Console.ReadLine() != null)
                    {
                        using (var channel = connection.CreateModel())
                        {

                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic.Add("x-expires", 30000);
                            dic.Add("x-message-ttl", 12000);//队列上消息过期时间，应小于队列过期时间  
                            dic.Add("x-dead-letter-exchange", "exchange-direct");//过期消息转向路由  
                            dic.Add("x-dead-letter-routing-key", "routing-delay");//过期消息转向路由相匹配routingkey  
                                                                                  //创建一个名叫"zzhello"的消息队列
                            channel.QueueDeclare(queue: "zzhello",
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: dic);

                            var message = "Hello World!";
                            var body = Encoding.UTF8.GetBytes(message);

                            //向该消息队列发送消息message
                            channel.BasicPublish(exchange: "",
                                routingKey: "zzhello",
                                basicProperties: null,
                                body: body);
                            Console.WriteLine(" [x] Sent {0}", message);
                        }
                    }
               
            }
        }
    }
}

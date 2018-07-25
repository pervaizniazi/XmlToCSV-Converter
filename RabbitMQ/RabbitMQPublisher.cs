using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ
{
    public class RabbitMQPublisher
    {
        public void SendMessage(IConfigurationSection section, string message)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connectionFactory.Port = Convert.ToInt32(section["Port"]);
            connectionFactory.HostName = section["HostName"].ToString();
            connectionFactory.UserName = section["Username"].ToString();
            connectionFactory.Password = section["Password"].ToString();
            connectionFactory.VirtualHost = section["VirtualHost"].ToString();

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "XMLToCSV",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                                 routingKey: "XMLToCSV",
                                                 basicProperties: null, body: body);

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Account.Infrastructure.Messaging
{
    public class RabbitMqConnection
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                DispatchConsumersAsync = true // permite uso de async/await nos consumidores

            };

            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();
            
            _channel.QueueDeclare(
                queue: "payment_info_request",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );
            
            _channel.QueueDeclare(
                queue: "payment_info_response",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );
            

        }

        public IModel GetChannel()
        {
            return _channel;
        }
    }
}
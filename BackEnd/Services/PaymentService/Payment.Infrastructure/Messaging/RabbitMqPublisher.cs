using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using Payment.Domain.Messaging;
using RabbitMQ.Client;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Payment.Infrastructure.Messaging
{
    public class RabbitMqPublisher : IMessagePublisher
    {
        private readonly RabbitMqConnection _connection;
        public RabbitMqPublisher(RabbitMqConnection Connection)
        {
            _connection = Connection;
        }

        public async Task PublicarAsync(string queue, string message, string correlationId = null)
        {
            var channel = _connection.GetChannel();
            var body = Encoding.UTF8.GetBytes(message);

            // Define a persistencia das Mensagens, caso o servidor reinicie, as mensagens continuam la
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true; // Make the message persistent

            if (!string.IsNullOrEmpty(correlationId))
            {
                properties.CorrelationId = correlationId;
            }
            
            channel.BasicPublish(
                exchange: "",
                routingKey: queue,
                basicProperties: properties,
                body: body
            );

        }
    }
}
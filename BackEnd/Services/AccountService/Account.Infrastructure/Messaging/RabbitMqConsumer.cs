using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Account.Domain.Interfaces;
using Account.Domain.Messaging;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Account.Infrastructure.Messaging
{
    public class RabbitMqConsumer<TEvent, TResponse> : IRabbitMqConsumer<TEvent, TResponse>
    {
        private readonly RabbitMqConnection _connection;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMqConsumer(RabbitMqConnection connection, IServiceScopeFactory serviceScopeFactory)
        {
            _connection = connection;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public virtual void StartConsuming()
        {
            var channel = _connection.GetChannel();

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                Console.WriteLine($"[x] Received message ea.Body: {ea.Body.ToArray()}");
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                Console.WriteLine($"[x] Received json: {json}");
                var jsonEventMessage = JsonSerializer.Deserialize<JsonElement>(json);
                Console.WriteLine($"[x] Received json desserializado: {jsonEventMessage}");

                jsonEventMessage.TryGetProperty("data", out JsonElement dataElement);
                var eventMessage = JsonSerializer.Deserialize<TEvent>(dataElement);
                Console.WriteLine($"[x] Received eventMessage: {eventMessage}");

                using var scope = _serviceScopeFactory.CreateScope();
                var consumo = scope.ServiceProvider.GetRequiredService<IMessageConsumer<TEvent, TResponse>>();
                await consumo.ConsumeMessage(eventMessage);
            };

            channel.BasicConsume(queue: "payment_info_request", autoAck: true, consumer: consumer);
        }
    }
}
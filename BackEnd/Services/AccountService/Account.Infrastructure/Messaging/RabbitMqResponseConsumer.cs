using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Account.Infrastructure.Messaging
{
public class RabbitMqResponseConsumer<TEvent, TResponse> : RabbitMqConsumer<TEvent, TResponse>, IRabbitMqResponseConsumer<TEvent, TResponse>
    {
        private readonly RabbitMqConnection _rabbitMqConnection;

        public RabbitMqResponseConsumer(RabbitMqConnection rabbitMqConnection, IServiceScopeFactory serviceScopeFactory) : base(rabbitMqConnection, serviceScopeFactory)
        {
            _rabbitMqConnection = rabbitMqConnection;
        }

        public async Task<string> ConsumeWithResponseAsync(string correlationId, string queueName)
        {
            var tcs = new TaskCompletionSource<string>();
            var channel = _rabbitMqConnection.GetChannel();
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var props = ea.BasicProperties;

                if (props.CorrelationId == correlationId)
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    tcs.TrySetResult(message);
                }
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            // Timeout para evitar espera infinita
            var timeoutTask = Task.Delay(10000); // 10 segundos
            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

            if (completedTask == timeoutTask)
            {
                throw new TimeoutException("Não foi recebida resposta no tempo esperado.");
            }

            return await tcs.Task;
        }
    }
}
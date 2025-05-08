using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Payment.Domain.Event;
using Payment.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Payment.Infrastructure.Messaging
{
    public class RabbitMqResponseConsumer<TEvent> : RabbitMqConsumer<TEvent>, IRabbitMqResponseConsumer<TEvent>
    {
        private readonly RabbitMqConnection _rabbitMqConnection;

        public RabbitMqResponseConsumer(RabbitMqConnection rabbitMqConnection, IServiceScopeFactory serviceScopeFactory)
            : base(rabbitMqConnection, serviceScopeFactory)
        {
            _rabbitMqConnection = rabbitMqConnection;
        }

        public async Task<TEvent> ConsumeWithResponseAsync(string correlationId, string queueName)
        {
            var tcs = new TaskCompletionSource<string>();
            using var channel = _rabbitMqConnection.GetChannel(); // ⚠️ canal agora é descartado após uso

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var props = ea.BasicProperties;

                Console.WriteLine($"[x] Esperando CorrelationId: {correlationId}");
                Console.WriteLine($"[x] Recebido CorrelationId: {props.CorrelationId}");

                if (props.CorrelationId == correlationId)
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);

                    tcs.TrySetResult(json);
                }
            };

            // Gerar consumer tag única e armazenar
            var consumerTag = channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            var timeoutTask = Task.Delay(10000); // 10 segundos
            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

            // Cancelar consumer após conclusão
            channel.BasicCancel(consumerTag); // ✅ Remove o listener antigo

            if (completedTask == timeoutTask)
            {
                throw new TimeoutException("Não foi recebida resposta no tempo esperado.");
            }

            var resultJson = await tcs.Task;

            // 🔄 Desserializar diretamente para o TEvent
            var result = JsonSerializer.Deserialize<TEvent>(
                JsonDocument.Parse(resultJson).RootElement.GetProperty("data").ToString()
            );

            return result!;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Domain.Interfaces
{
    public interface IRabbitMqResponseConsumer<TEvent> : IRabbitMqConsumer<TEvent>
    {
        Task<TEvent> ConsumeWithResponseAsync(string correlationId, string queueName);
    }
}
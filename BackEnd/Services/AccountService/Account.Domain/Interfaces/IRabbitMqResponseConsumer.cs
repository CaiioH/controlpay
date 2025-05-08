using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Domain.Interfaces
{
    public interface IRabbitMqResponseConsumer<TEvent, TResponse> : IRabbitMqConsumer<TEvent, TResponse>
    {
        Task<string> ConsumeWithResponseAsync(string correlationId, string queueName);
    }
}
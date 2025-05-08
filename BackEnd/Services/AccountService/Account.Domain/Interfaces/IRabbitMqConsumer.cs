using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Domain.Interfaces
{
    public interface IRabbitMqConsumer<TEvent, TResponse>
    {
        void StartConsuming();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Domain.Interfaces
{
    public interface IRabbitMqConsumer<TEvent>
    {
        void StartConsuming();
    }
}
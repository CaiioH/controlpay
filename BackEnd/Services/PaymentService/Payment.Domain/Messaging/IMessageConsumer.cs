using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Domain.Messaging
{
    public interface IMessageConsumer<TEvent>
    {
        Task<TEvent> ConsumeMessage(TEvent message);
    }
}
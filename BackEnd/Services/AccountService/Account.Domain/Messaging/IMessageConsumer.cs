using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Domain.Messaging
{
    public interface IMessageConsumer<TRequest, TResponse>
    {
        Task<TResponse> ConsumeMessage(TRequest eventMessage);
    }
}
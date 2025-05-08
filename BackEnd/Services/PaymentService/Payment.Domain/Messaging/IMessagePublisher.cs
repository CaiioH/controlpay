using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Domain.Messaging
{
    public interface IMessagePublisher
    {
        Task PublicarAsync(string queue, string message, string correlationId = null);
    }
}
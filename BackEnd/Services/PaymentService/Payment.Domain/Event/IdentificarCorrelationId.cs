using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Domain.Event
{
    public class IdentificarCorrelationId
    {
        public string type { get; set; }
        public string CorrelationId { get; set; }
        public object data { get; set; }

    }
}
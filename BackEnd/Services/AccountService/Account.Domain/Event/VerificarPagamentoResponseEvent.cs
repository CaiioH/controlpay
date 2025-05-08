using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Domain.Event
{
    public class VerificarPagamentoResponseEvent
    {
        public int Id { get; set; }
        public string CorrelationId { get; set; }
        public bool Pago { get; set; }
    }
}
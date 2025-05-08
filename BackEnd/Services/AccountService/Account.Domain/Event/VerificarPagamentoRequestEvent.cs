using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Domain.Event
{
    public class VerificarPagamentoRequestEvent
    {
        public int Id { get; set; }
        public string CorrelationId { get; set; }
        public decimal ValorPago { get; set; }
    }
}
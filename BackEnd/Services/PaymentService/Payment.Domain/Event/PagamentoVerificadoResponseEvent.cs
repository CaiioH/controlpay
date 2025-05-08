using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Domain.Event
{
    public class PagamentoVerificadoResponseEvent
    {
        public int Id { get; set; }
        public bool Pago { get; set; }
        public string Response { get; set; }
        public decimal ValorParcela { get; set; }
    }
}
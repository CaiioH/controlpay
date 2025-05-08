using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Application.DTOs
{
    public class PagamentoCreateDTO
    {
        public int ParcelaId { get; set; }
        public decimal ValorPago { get; set; }
        public string ComprovanteUrl { get; set; }
        public DateTime DataPagamento { get; set; } = DateTime.UtcNow;
    }
}
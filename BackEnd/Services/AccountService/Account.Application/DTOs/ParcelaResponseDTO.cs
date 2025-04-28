using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Application.DTOs
{
    public class ParcelaResponseDTO
    {
        public int Id { get; set; }
        public int IdConta { get; set; }
        public int NumeroParcela { get; set; }
        public decimal ValorParcela { get; set; }
        public DateTime DataVencimento { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Application.DTOs
{
    public class ContaDTO
    {
        public string Nome { get; set; }
        public decimal ValorTotal { get; set; }
        public int QtParcelas { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
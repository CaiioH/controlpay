using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Application.DTOs
{
    public class ContaUpdateDTO
    {
        public string Nome { get; set; }
        public decimal ValorTotal { get; set; }
        public int QtParcelas { get; set; }
        public bool Ativo { get; set; }
    }
}
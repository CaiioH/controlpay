using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Application.DTOs
{
    public class ContaCreateDTO
    {
        public string Nome { get; set; }
        public decimal ValorTotal { get; set; }
        public int QtParcelas { get; set; }
        public DateTime DataVencimento { get; set; } = DateTime.UtcNow.AddDays(30); // Definindo um valor padrão de 30 dias a partir de agora
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Domain.Models;

namespace Account.Domain.Factory
{
    public static class ContaFactory
    {
        public static IEnumerable<Parcela> GerarParcelas(Conta conta)
        {
            var parcelas = new List<Parcela>();
            decimal valorParcela = conta.ValorTotal / conta.QtParcelas;
            int quantidadeParcelas = conta.QtParcelas;

            for (int i = 1; i < quantidadeParcelas+1; i++)
            {
                parcelas.Add(new Parcela
                (
                    conta.Id,
                    i,
                    valorParcela,
                    conta.DataVencimento.AddDays(i * 30) // Adicionando 30 dias para cada parcela
                ));
            }

            return parcelas;
        }
    }
}
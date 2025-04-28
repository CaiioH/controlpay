using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Domain.Models
{
    public class Parcela
    {
        public int Id { get; private set; }
        public int NumeroParcela { get; private set; }
        public decimal ValorParcela { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public bool Paga { get; private set; } = false;

        // Relacionamento com a entidade Conta
        public int ContaId { get; private set; }
        public Conta Conta { get; private set; }
        

        public Parcela() { }
        public Parcela(int contaId, int numeroParcela, decimal valorParcela, DateTime dataVencimento)
        {
            DefinirIdConta(contaId);
            DefinirNumeroParcela(numeroParcela);
            DefinirValorParcela(valorParcela);
            DefinirDataVencimento(dataVencimento);
            Paga = false;
        }

        public void MarcarComoPaga()
        {
            Paga = true;
        }
        public void DefinirIdConta(int idConta)
        {
            if (idConta <= 0)
                throw new ArgumentException("Id da conta não pode ser menor ou igual a zero.");

            ContaId = idConta;
        }
        public void DefinirNumeroParcela(int numeroParcela)
        {
            if (numeroParcela <= 0)
                throw new ArgumentException("Número da parcela não pode ser menor ou igual a zero.");

            NumeroParcela = numeroParcela;
        }
        public void DefinirValorParcela(decimal valorParcela)
        {
            if (valorParcela <= 0)
                throw new ArgumentException("Valor da parcela não pode ser menor ou igual a zero.");

            ValorParcela = valorParcela;
        }
        public void DefinirDataVencimento(DateTime dataVencimento)
        {
            if (dataVencimento < DateTime.UtcNow)
                throw new ArgumentException("Data de vencimento não pode ser no passado.");

            DataVencimento = dataVencimento;
        }
        public void AtualizarDados(int numeroParcela, decimal valorParcela, DateTime dataVencimento)
        {
            DefinirNumeroParcela(numeroParcela);
            DefinirValorParcela(valorParcela);
            DefinirDataVencimento(dataVencimento);
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Domain.Models
{
    public class Conta
    {
        public int Id { get; private set; }
        public int IdUsuario { get; private set; }
        public string Nome { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public int QtParcelas { get; private set; }
        public DateTime DataVencimento { get; private set; }
        public bool Ativo { get; private set; } = true;

        // Relacionamento com a entidade Parcela
        public ICollection<Parcela> Parcelas { get; private set; } = new List<Parcela>();

        public Conta() { }

        public Conta(int idUsuario, string nome, decimal valorTotal, int qtParcelas, DateTime dataVencimento)
        {
            DefinirIdUsuario(idUsuario);
            DefinirNome(nome);
            DefinirValorTotal(valorTotal);
            DefinirQtParcelas(qtParcelas);
            DataCriacao = DateTime.UtcNow;
            DefinirDataVencimento(dataVencimento);
            Ativo = true;
        }

        private void DefinirIdUsuario(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("Id do usuário não pode ser menor ou igual a zero.");

            IdUsuario = idUsuario;
        }
        private void DefinirNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("Nome não pode ser vazio ou nulo.");

            Nome = nome;
        }
        private void DefinirValorTotal(decimal valorTotal)
        {
            if (valorTotal <= 0)
                throw new ArgumentException("Valor total não pode ser menor ou igual a zero.");

            ValorTotal = valorTotal;
        }
        private void DefinirQtParcelas(int qtParcelas)
        {
            if (qtParcelas <= 0)
                throw new ArgumentException("Quantidade de parcelas não pode ser menor ou igual a zero.");

            QtParcelas = qtParcelas;

        }
        public void AtualizarDados(decimal valorTotal, int qtParcelas, string nome)
        {
            DefinirNome(nome);
            DefinirValorTotal(valorTotal);
            DefinirQtParcelas(qtParcelas);
        }
        public void AtualizarNome(string nome)
        {
            DefinirNome(nome);
        }

        public void AtualizarValorTotal(decimal valorTotal)
        {
            DefinirValorTotal(valorTotal);
        }
        public void AtualizarQtParcelas(int qtParcelas)
        {
            DefinirQtParcelas(qtParcelas);
        }
        public void AdicionarParcelas(IEnumerable<Parcela> parcelas)
        {
            Parcelas = parcelas.ToList();
        }

        public void DefinirDataVencimento(DateTime dataVencimento)
        {
            if (dataVencimento < DateTime.UtcNow)
                throw new ArgumentException("Data de vencimento não pode ser no passado.");

            DataVencimento = dataVencimento;
        }
        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;

    }
}
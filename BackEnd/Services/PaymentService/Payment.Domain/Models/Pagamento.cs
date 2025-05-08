using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.Domain.Models
{
    public class Pagamento
    {
        public int id { get; private set; }
        public int ParcelaId { get; private set; }
        public decimal ValorPago { get; private set; }    
        public DateTime DataPagamento { get; private set; }
        public string ComprovanteUrl { get; private set; }
        
        public Pagamento(){ }

        public Pagamento(int parcelaId, decimal valorPago, string comprovanteUrl)
        {
            DefinirParcelaId(parcelaId);
            DefinirValorPago(valorPago);
            DefinirComprovanteUrl(comprovanteUrl);
            DataPagamento = DateTime.UtcNow;
        }

        public void DefinirParcelaId(int parcelaId)
        {
            if (parcelaId <= 0)
                throw new ArgumentException("ID da parcela não pode ser menor ou igual a zero.");

            ParcelaId = parcelaId;
        }
        public void DefinirValorPago(decimal valorPago)
        {
            if (valorPago <= 0)
                throw new ArgumentException("Valor pago não pode ser menor ou igual a zero.");

            ValorPago = valorPago;
        }

        public void DefinirComprovanteUrl(string comprovanteUrl)
        {
            if (string.IsNullOrWhiteSpace(comprovanteUrl))
                throw new ArgumentException("URL do comprovante não pode ser vazia ou nula.");

            ComprovanteUrl = comprovanteUrl;
        }
        
        public void AtualizarValorPago(decimal valorPago)
        {
            DefinirValorPago(valorPago);
        }
        public void AtualizarComprovanteUrl(string comprovanteUrl)
        {
            DefinirComprovanteUrl(comprovanteUrl);
        }
        public void AtualizarDataPagamento(DateTime dataPagamento)
        {
            DataPagamento = dataPagamento;
        }
        public void CancelarPagamento()
        {
            // Lógica para cancelar o pagamento, se necessário
            // Por exemplo, definir um status de cancelado ou remover o pagamento
        }

        public void AtualizarDados(decimal valorPago, string comprovanteUrl, DateTime dataPagamento)
        {
            DefinirValorPago(valorPago);
            DefinirComprovanteUrl(comprovanteUrl);
            AtualizarDataPagamento(dataPagamento);
        }

    }
}
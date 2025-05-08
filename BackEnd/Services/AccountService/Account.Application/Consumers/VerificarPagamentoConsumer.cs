using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Account.Domain.Event;
using Account.Domain.Interfaces;
using Account.Domain.Messaging;

namespace Account.Application.Consumers
{
    public class VerificarPagamentoConsumer : IMessageConsumer<VerificarPagamentoRequestEvent, VerificarPagamentoResponseEvent>
    {
        private readonly IParcelaRepository _parcelaRepository;
        private readonly IMessagePublisher _messagePublisher;

        public VerificarPagamentoConsumer(IMessagePublisher messagePublisher, IParcelaRepository parcelaRepository)
        {
            _messagePublisher = messagePublisher;
            _parcelaRepository = parcelaRepository;
        }

        public async Task<VerificarPagamentoResponseEvent> ConsumeMessage(VerificarPagamentoRequestEvent eventMessage)
        {
            Console.WriteLine($"[✓] Mensagem recebida: Id = {eventMessage.Id}, CorrelationId = {eventMessage.CorrelationId}");

            string response = "";
            var parcela = await _parcelaRepository.ObterParcelaByIdAsync(eventMessage.Id);
            bool foiPaga = parcela?.Paga ?? false;

            if (parcela == null)
            {
                response = $"Parcela com ID {eventMessage.Id} não encontrada.";
            }
            else if (parcela.ValorParcela == eventMessage.ValorPago && foiPaga == false)
            {
                response = "Pagamento realizado com sucesso.";
                parcela.MarcarComoPaga();
                await _parcelaRepository.AtualizarParcelaAsync(parcela);
            }
            else if (foiPaga == false)
            {
                if (parcela.ValorParcela != eventMessage.ValorPago)
                {
                    response = "Valor do pagamento não corresponde ao valor da parcela.";
                }
            }
            else
            {
                response = "Já consta o pagamento dessa parcela.";
            }

            var msg = new
            {
                type = "payment_info_response",
                correlationId = eventMessage.CorrelationId,
                data = new
                {
                    Id = eventMessage.Id,
                    Pago = foiPaga,
                    Response = response,
                    ValorParcela = parcela?.ValorParcela
                }
            };

            string jsonString = System.Text.Json.JsonSerializer.Serialize(msg);
            Console.WriteLine($"[→] Enviando resposta: {jsonString}");

            // ✅ Publicação da resposta com CorrelationId no header (RabbitMQ)
            await _messagePublisher.PublicarAsync(msg.type, jsonString, eventMessage.CorrelationId);

            return new VerificarPagamentoResponseEvent
            {
                Id = eventMessage.Id,
                CorrelationId = eventMessage.CorrelationId,
                Pago = foiPaga
            };
        }
    }
}

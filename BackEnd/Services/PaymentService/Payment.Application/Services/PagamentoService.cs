using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Payment.Application.DTOs;
using Payment.Application.Interfaces;
using Payment.Domain.Event;
using Payment.Domain.Interfaces;
using Payment.Domain.Messaging;
using Payment.Domain.Models;
namespace Payment.Application.Services
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IMessagePublisher _messagePublisher;
        private readonly IRabbitMqResponseConsumer<PagamentoVerificadoResponseEvent> _messageConsumer;
        public PagamentoService(IPagamentoRepository pagamentoRepository, IMessagePublisher messagePublisher, IRabbitMqResponseConsumer<PagamentoVerificadoResponseEvent> messageConsumer)
        {
            _messageConsumer = messageConsumer;
            _pagamentoRepository = pagamentoRepository;
            _messagePublisher = messagePublisher;
        }


        public async Task<PagamentoResponseDTO> RegistrarPagamentoAsync(PagamentoCreateDTO pagamentoCreateDTO)
        {
            try
            {
                // Gerar um CorrelationId único para rastrear a resposta
                var correlationId = Guid.NewGuid().ToString();
                Console.WriteLine($"CorrelationId gerado: {correlationId}");
                var msg = new
                {
                    type = "payment_info_request",
                    data = new
                    {
                        CorrelationId = correlationId, // Incluindo o correlationId
                        Id = pagamentoCreateDTO.ParcelaId,
                        ValorPago = pagamentoCreateDTO.ValorPago
                    }
                };

                string jsonString = JsonSerializer.Serialize(msg);

                await _messagePublisher.PublicarAsync(msg.type, jsonString, correlationId);

                // Aqui eu poderia fazer um await para esperar a resposta do serviço de parcelas
                var resposta = await _messageConsumer.ConsumeWithResponseAsync(correlationId, "payment_info_response");
                Console.WriteLine($"Resposta recebida: Id: {resposta.Id} - Pago: {resposta.Pago} - Response: {resposta.Response}");
                if (resposta == null || resposta.Pago == true || resposta.ValorParcela != pagamentoCreateDTO.ValorPago)
                    throw new Exception(resposta.Response);

                var pagamento = new Pagamento
                (
                    pagamentoCreateDTO.ParcelaId,
                    pagamentoCreateDTO.ValorPago,
                    pagamentoCreateDTO.ComprovanteUrl
                );

                await _pagamentoRepository.AdicionarPagamentoAsync(pagamento);

                // Falta enviar a mensagem de confirmação de pagamento para o RabbitMQ

                return new PagamentoResponseDTO
                {
                    Id = pagamento.id,
                    ValorPago = pagamento.ValorPago,
                    DataPagamento = pagamento.DataPagamento,
                    ParcelaId = pagamento.ParcelaId
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao registrar pagamento: {ex.Message}", ex);
            }
        }
        public async Task<PagamentoResponseDTO> AtualizarPagamentoAsync(int id, PagamentoCreateDTO pagamentoUpdateDTO)
        {
            try
            {
                var pagamento = await _pagamentoRepository.ObterPagamentoByIdAsync(id);
                if (pagamento == null)
                    throw new Exception("Pagamento não encontrado.");

                pagamento.AtualizarDados(pagamentoUpdateDTO.ValorPago, pagamentoUpdateDTO.ComprovanteUrl, pagamentoUpdateDTO.DataPagamento);

                await _pagamentoRepository.AtualizarPagamentoAsync(pagamento);

                return new PagamentoResponseDTO
                {
                    Id = pagamento.id,
                    ValorPago = pagamento.ValorPago,
                    DataPagamento = pagamento.DataPagamento,
                    ParcelaId = pagamento.ParcelaId
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar pagamento: {ex.Message}", ex);
            }
        }

        public async Task<ICollection<PagamentoResponseDTO>> ListarPagamentosPorParcelaIdAsync(int parcelaId)
        {
            try
            {
                // Talvez eu tenha que enviar mensagem pra verificar se a parcela realmente existe
                // e se o pagamento não foi feito antes

                var pagamentos = await _pagamentoRepository.ListarPagamentosPorParcelaIdAsync(parcelaId);
                return pagamentos.Select(p => new PagamentoResponseDTO
                {
                    Id = p.id,
                    ValorPago = p.ValorPago,
                    DataPagamento = p.DataPagamento,
                    ParcelaId = p.ParcelaId
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar pagamentos por parcela: {ex.Message}", ex);
            }
        }

        public async Task<ICollection<PagamentoResponseDTO>> ListarTodosPagamentosAsync()
        {
            try
            {
                var pagamentos = await _pagamentoRepository.ListarTodosPagamentosAsync();
                return pagamentos.Select(p => new PagamentoResponseDTO
                {
                    Id = p.id,
                    ValorPago = p.ValorPago,
                    DataPagamento = p.DataPagamento,
                    ParcelaId = p.ParcelaId
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar todos os pagamentos: {ex.Message}", ex);
            }
        }

        public async Task<PagamentoResponseDTO> ObterPagamentoPorIdAsync(int id)
        {
            try
            {
                var pagamento = await _pagamentoRepository.ObterPagamentoByIdAsync(id);
                if (pagamento == null)
                    throw new Exception("Pagamento não encontrado.");

                return new PagamentoResponseDTO
                {
                    Id = pagamento.id,
                    ValorPago = pagamento.ValorPago,
                    DataPagamento = pagamento.DataPagamento,
                    ParcelaId = pagamento.ParcelaId
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter pagamento: {ex.Message}", ex);
            }
        }

        public async Task<bool> RemoverPagamentoAsync(int id)
        {
            try
            {
                var pagamento = await _pagamentoRepository.ObterPagamentoByIdAsync(id);
                if (pagamento == null)
                    throw new Exception("Pagamento não encontrado.");


                await _pagamentoRepository.RemoverPagamentoAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao remover pagamento: {ex.Message}", ex);
            }
        }
    }
}
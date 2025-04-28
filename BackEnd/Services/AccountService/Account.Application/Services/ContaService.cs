using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Account.Application.DTOs;
using Account.Application.Interfaces;
using Account.Domain.Extensions;
using Account.Domain.Factory;
using Account.Domain.Interfaces;
using Account.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Account.Application.Services
{
    public class ContaService : IContaService
    {
        private readonly IContaRepository _contaRepository;
        private readonly IParcelaRepository _parcelaRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ContaService(IContaRepository contaRepository, IParcelaRepository parcelaRepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _contaRepository = contaRepository;
            _parcelaRepository = parcelaRepository;
        }

        public async Task<ContaResponseDTO> CriarContaAsync(ContaCreateDTO contaDTO)
        {
            try
            {
                var idUsuarioLogado = _httpContextAccessor.HttpContext.User.ObterUserIdByTokenJWT();

                var conta = new Conta
                (
                    // preciso pegar o id do usuario logado
                    idUsuarioLogado,
                    contaDTO.Nome,
                    contaDTO.ValorTotal,
                    contaDTO.QtParcelas,
                    contaDTO.DataVencimento
                );

                await _contaRepository.AdicionarContaAsync(conta);
                
                // Gerar parcelas e adicionar na conta
                var parcelas = ContaFactory.GerarParcelas(conta);
                conta.AdicionarParcelas(parcelas);

                await _parcelaRepository.AdicionarVariasParcelasAsync(parcelas);

                return new ContaResponseDTO
                {
                    Id = conta.Id,
                    Nome = conta.Nome,
                    ValorTotal = conta.ValorTotal,
                    QtParcelas = conta.QtParcelas,
                    DataCriacao = conta.DataCriacao
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar conta: {ex.Message}", ex);
            }
        }

        public async Task<ContaDTO> ObterContaPorIdAsync(int id)
        {
            try
            {
                var conta = await _contaRepository.ObterContaByIdAsync(id);
                if (conta == null) 
                    throw new Exception("Conta não encontrada.");

                return new ContaDTO
                {
                    Nome = conta.Nome,
                    ValorTotal = conta.ValorTotal,
                    QtParcelas = conta.QtParcelas,
                    DataCriacao = conta.DataCriacao
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter conta: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<ContaDTO>> ObterTodasContasAsync()
        {
            try
            {
                var contas = await _contaRepository.ObterTodasContasAsync();
                return contas.Select(c => new ContaDTO
                {
                    Nome = c.Nome,
                    ValorTotal = c.ValorTotal,
                    QtParcelas = c.QtParcelas,
                    DataCriacao = c.DataCriacao
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar contas: {ex.Message}", ex);
            }
        }

        public async Task<ContaUpdateDTO> AtualizarContaAsync(int id, ContaUpdateDTO contaDTO)
        {
            try
            {
                var conta = await _contaRepository.ObterContaByIdAsync(id);
                if (conta == null) 
                    throw new Exception("Conta não encontrada.");

                conta.AtualizarDados(contaDTO.ValorTotal, contaDTO.QtParcelas, contaDTO.Nome);
                await _contaRepository.AtualizarContaAsync(conta);

                return new ContaUpdateDTO
                {
                    Nome = conta.Nome,
                    ValorTotal = conta.ValorTotal,
                    QtParcelas = conta.QtParcelas,
                    Ativo = conta.Ativo
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar conta: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeletarContaAsync(int id)
        {
            try
            {
                var conta = await _contaRepository.ObterContaByIdAsync(id);
                if (conta == null) 
                    throw new Exception("Conta não encontrada.");

                await _contaRepository.DeletarContaAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar conta: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<ContaDTO>> ObterContasPorClienteIdAsync(int clienteId)
        {
            try
            {
                var contas = await _contaRepository.ObterContasPorClienteIdAsync(clienteId);
                return contas.Select(c => new ContaDTO
                {
                    Nome = c.Nome,
                    ValorTotal = c.ValorTotal,
                    QtParcelas = c.QtParcelas,
                    DataCriacao = c.DataCriacao
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar contas por cliente: {ex.Message}", ex);
            }
        }

    }
}
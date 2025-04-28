using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Application.DTOs;
using Account.Application.Interfaces;
using Account.Domain.Interfaces;
using Account.Domain.Models;

namespace Account.Application.Services
{
    public class ParcelaService : IParcelaService
    {
        private readonly IParcelaRepository _parcelaRepository;
        
        public ParcelaService(IParcelaRepository parcelaRepository)
        {
            _parcelaRepository = parcelaRepository;
        }

        public async Task<ParcelaResponseDTO> AdicionarParcelaAsync(ParcelaCreateDTO createDTO)
        {
            try
            {
                var parcela = new Parcela
                (
                    createDTO.IdConta,
                    createDTO.NumeroParcela,
                    createDTO.ValorParcela,
                    createDTO.DataVencimento
                );

                await _parcelaRepository.AdicionarParcelaAsync(parcela);
                
                return new ParcelaResponseDTO
                {
                    Id = parcela.Id,
                    IdConta = parcela.ContaId,
                    NumeroParcela = parcela.NumeroParcela,
                    ValorParcela = parcela.ValorParcela,
                    DataVencimento = parcela.DataVencimento
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar parcela: {ex.Message}", ex);
            }
        }

        public async Task<ParcelaDTO> AtualizarParcelaAsync(int id, ParcelaCreateDTO dto)
        {
            try
            {
                var parcela = await _parcelaRepository.ObterParcelaByIdAsync(id);
                if (parcela == null)
                {
                    throw new Exception("Parcela não encontrada.");
                }

                parcela.AtualizarDados(dto.NumeroParcela, dto.ValorParcela, dto.DataVencimento);
                await _parcelaRepository.AtualizarParcelaAsync(parcela);
                
                return new ParcelaDTO
                {
                    NumeroParcela = parcela.NumeroParcela,
                    ValorParcela = parcela.ValorParcela,
                    DataVencimento = parcela.DataVencimento
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar parcela: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeletarParcelaAsync(int id)
        {
            try
            {
                var parcela = await _parcelaRepository.ObterParcelaByIdAsync(id);
                if (parcela == null)
                {
                    throw new Exception("Parcela não encontrada.");
                }

                await _parcelaRepository.DeletarParcelaAsync(id);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar parcela: {ex.Message}", ex);
            }
        }

        public async Task<ParcelaDTO> ObterParcelaByIdAsync(int id)
        {
            try
            {
                var parcela = await _parcelaRepository.ObterParcelaByIdAsync(id);
                if (parcela == null)
                {
                    throw new Exception("Parcela não encontrada.");
                }

                return new ParcelaDTO
                {
                    NumeroParcela = parcela.NumeroParcela,
                    ValorParcela = parcela.ValorParcela,
                    DataVencimento = parcela.DataVencimento
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter parcela: {ex.Message}", ex);
            }	
        }

        public async Task<IEnumerable<Parcela>> ObterParcelasPorContaIdAsync(int contaId)
        {
            try
            {
                var parcelas = await _parcelaRepository.ObterParcelasPorContaIdAsync(contaId);
                if (parcelas == null)
                {
                    throw new Exception("Parcelas não encontradas.");
                }
                return parcelas;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter parcelas: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<ParcelaDTO>> ObterTodasParcelasAsync()
        {
            try
            {
                var parcelas = await _parcelaRepository.ObterTodasParcelasAsync();
                return parcelas.Select(p => new ParcelaDTO
                {
                    NumeroParcela = p.NumeroParcela,
                    ValorParcela = p.ValorParcela,
                    DataVencimento = p.DataVencimento
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar parcelas: {ex.Message}", ex);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Application.DTOs;

namespace Account.Application.Interfaces
{
    public interface IContaService
    {
        Task<ContaResponseDTO> CriarContaAsync(ContaCreateDTO contaDTO);
        Task<ContaDTO> ObterContaPorIdAsync(int id);
        Task<IEnumerable<ContaDTO>> ObterTodasContasAsync();
        Task<bool> DeletarContaAsync(int id);
        Task<ContaUpdateDTO> AtualizarContaAsync(int id, ContaUpdateDTO contaDTO);
        Task<IEnumerable<ContaDTO>> ObterContasPorClienteIdAsync(int clienteId);
    }
}
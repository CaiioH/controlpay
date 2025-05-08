using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payment.Application.DTOs;

namespace Payment.Application.Interfaces
{
    public interface IPagamentoService
    {
        Task<PagamentoResponseDTO> RegistrarPagamentoAsync(PagamentoCreateDTO pagamentoCreateDTO);
        Task<ICollection<PagamentoResponseDTO>> ListarPagamentosPorParcelaIdAsync(int parcelaId);
        Task<PagamentoResponseDTO> ObterPagamentoPorIdAsync(int id);
        Task<PagamentoResponseDTO> AtualizarPagamentoAsync(int id, PagamentoCreateDTO pagamentoUpdateDTO);
        Task<bool> RemoverPagamentoAsync(int id);
        Task<ICollection<PagamentoResponseDTO>> ListarTodosPagamentosAsync();

    }
}
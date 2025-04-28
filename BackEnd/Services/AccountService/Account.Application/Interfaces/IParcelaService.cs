using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Application.DTOs;
using Account.Domain.Models;

namespace Account.Application.Interfaces
{
    public interface IParcelaService
    {
        Task<ParcelaResponseDTO> AdicionarParcelaAsync(ParcelaCreateDTO parcelaCreateDTO);
        Task<ParcelaDTO> ObterParcelaByIdAsync(int id);
        Task<IEnumerable<ParcelaDTO>> ObterTodasParcelasAsync();
        Task<ParcelaDTO> AtualizarParcelaAsync(int id, ParcelaCreateDTO parcelaUpdateDTO);
        Task<bool> DeletarParcelaAsync(int id);
        Task<IEnumerable<Parcela>> ObterParcelasPorContaIdAsync(int contaId);
    }
}
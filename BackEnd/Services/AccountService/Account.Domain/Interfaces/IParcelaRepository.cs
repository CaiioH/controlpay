using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Domain.Models;

namespace Account.Domain.Interfaces
{
    public interface IParcelaRepository
    {
        Task AdicionarParcelaAsync(Parcela parcela);
        Task AdicionarVariasParcelasAsync(IEnumerable<Parcela> parcelas);
        Task<Parcela> ObterParcelaByIdAsync(int id);
        Task<IEnumerable<Parcela>> ObterTodasParcelasAsync();
        Task AtualizarParcelaAsync(Parcela parcela);
        Task DeletarParcelaAsync(int id);
        Task<IEnumerable<Parcela>> ObterParcelasPorContaIdAsync(int contaId);
    }
}
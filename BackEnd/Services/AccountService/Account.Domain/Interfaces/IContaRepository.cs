using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Domain.Models;

namespace Account.Domain.Interfaces
{
    public interface IContaRepository
    {
        Task AdicionarContaAsync(Conta conta);
        Task<Conta> ObterContaByIdAsync(int id);
        Task<IEnumerable<Conta>> ObterTodasContasAsync();
        Task AtualizarContaAsync(Conta conta);
        Task DeletarContaAsync(int id);
        Task<IEnumerable<Conta>> ObterContasPorClienteIdAsync(int clienteId);
        
    }
}
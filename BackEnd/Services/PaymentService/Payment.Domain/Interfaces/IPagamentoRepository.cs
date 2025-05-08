using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Payment.Domain.Models;

namespace Payment.Domain.Interfaces
{
    public interface IPagamentoRepository
    {
        Task AdicionarPagamentoAsync(Pagamento pagamento);
        Task<Pagamento> ObterPagamentoByIdAsync(int id);
        Task<ICollection<Pagamento>> ListarTodosPagamentosAsync();
        Task<bool> RemoverPagamentoAsync(int id);
        Task AtualizarPagamentoAsync(Pagamento pagamento);
        Task<ICollection<Pagamento>> ListarPagamentosPorParcelaIdAsync(int parcelaId);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payment.Domain.Interfaces;
using Payment.Domain.Models;
using Payment.Infrastructure.Data;

namespace Payment.Infrastructure.Repositories
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly PaymentDbContext _context;
        public PagamentoRepository(PaymentDbContext context)
        {
            _context = context;
        }
        public async Task AdicionarPagamentoAsync(Pagamento pagamento)
        {
            try
            {
                _context.Pagamentos.Add(pagamento);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar pagamento: {ex.Message}", ex);
            }
        }

        public async Task AtualizarPagamentoAsync(Pagamento pagamento)
        {
            try
            {
                _context.Pagamentos.Update(pagamento);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar pagamento: {ex.Message}", ex);
            }
        }

        public async Task<ICollection<Pagamento>> ListarPagamentosPorParcelaIdAsync(int parcelaId)
        {
            try
            {
                return await _context.Pagamentos.Where(p => p.ParcelaId == parcelaId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar pagamentos por parcela: {ex.Message}", ex);
            }
        }

        public async Task<ICollection<Pagamento>> ListarTodosPagamentosAsync()
        {
            try
            {
                return await _context.Pagamentos.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar pagamentos: {ex.Message}", ex);
            }
        }

        public async Task<Pagamento> ObterPagamentoByIdAsync(int id)
        {
            try
            {
                return await _context.Pagamentos.FindAsync(id);
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
                var pagamento = await ObterPagamentoByIdAsync(id);
                if (pagamento == null) return false;

                _context.Pagamentos.Remove(pagamento);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao remover pagamento: {ex.Message}", ex);
            }
        }
    }
}
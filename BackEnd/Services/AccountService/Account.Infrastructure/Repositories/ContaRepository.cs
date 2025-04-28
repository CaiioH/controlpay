using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Domain.Interfaces;
using Account.Domain.Models;
using Account.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly AccountDbContext _context;
        public ContaRepository(AccountDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarContaAsync(Conta conta)
        {
            try
            {
                await _context.Contas.AddAsync(conta);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar conta: {ex.Message}", ex);
            }
        }

        public async Task<Conta> ObterContaByIdAsync(int id)
        {
            try
            {
                return await _context.Contas.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter conta: {ex.Message}", ex);
            }
        }
        public async Task<IEnumerable<Conta>> ObterTodasContasAsync()
        {
            try
            {
                return await _context.Contas.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar contas: {ex.Message}", ex);
            }
        }
        public async Task AtualizarContaAsync(Conta conta)
        {
            try
            {
                _context.Contas.Update(conta);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar conta: {ex.Message}", ex);
            }
        }

        public async Task DeletarContaAsync(int id)
        {
            try
            {
                var conta = await ObterContaByIdAsync(id);
                if (conta != null)
                {
                    _context.Contas.Remove(conta);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao remover conta: {ex.Message}", ex);
            }
        }
        public async Task<IEnumerable<Conta>> ObterContasPorClienteIdAsync(int clienteId)
        {
            try
            {
                return await _context.Contas.Where(c => c.IdUsuario == clienteId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter contas por cliente: {ex.Message}", ex);
            }
        }

    }
}
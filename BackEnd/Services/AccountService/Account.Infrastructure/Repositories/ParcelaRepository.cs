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
    public class ParcelaRepository : IParcelaRepository
    {
        private readonly AccountDbContext _context;
        public ParcelaRepository(AccountDbContext context)
        {
            _context = context;
        }
        public async Task AdicionarParcelaAsync(Parcela parcela)
        {
            try
            {
                await _context.Parcelas.AddAsync(parcela);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar parcela: {ex.Message}", ex);
            }
        }

        public async Task AdicionarVariasParcelasAsync(IEnumerable<Parcela> parcelas)
        {
            try
            {
                await _context.Parcelas.AddRangeAsync(parcelas);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao adicionar várias parcelas: {ex.Message}", ex);
            }
        }
        public async Task<Parcela> ObterParcelaByIdAsync(int id)
        {

            try
            {
                return await _context.Parcelas.FindAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter parcela: {ex.Message}", ex);
            }
        }
        public async Task<IEnumerable<Parcela>> ObterTodasParcelasAsync()
        {

            try
            {
                return await _context.Parcelas.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter todas as parcelas: {ex.Message}", ex);
            }
        }
        public async Task AtualizarParcelaAsync(Parcela parcela)
        {

            try
            {
                _context.Parcelas.Update(parcela);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao atualizar parcela: {ex.Message}", ex);
            }
        }
        public async Task DeletarParcelaAsync(int id)
        {

            try
            {
                var parcela = await _context.Parcelas.FindAsync(id);
                if (parcela != null)
                {
                    _context.Parcelas.Remove(parcela);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar parcela: {ex.Message}", ex);
            }
        }
        public async Task<IEnumerable<Parcela>> ObterParcelasPorContaIdAsync(int contaId)
        {
            try
            {
                return await _context.Parcelas.Where(p => p.ContaId == contaId).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter parcelas por conta ID: {ex.Message}", ex);
            }
        }
    }
}
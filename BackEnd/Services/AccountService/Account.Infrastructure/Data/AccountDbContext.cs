using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.Data
{
    public class AccountDbContext : DbContext
    {
        public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options)
        { }

        public DbSet<Conta> Contas { get; set; }
        public DbSet<Parcela> Parcelas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Conta>(entity =>
            {
                entity.Property(c => c.ValorTotal)
                      .HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Parcela>(entity =>
            {
                entity.Property(p => p.ValorParcela)
                      .HasColumnType("decimal(18,2)");
            });
        }

    }
}
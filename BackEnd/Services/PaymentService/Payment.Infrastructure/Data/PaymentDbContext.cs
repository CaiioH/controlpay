using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Payment.Domain.Models;

namespace Payment.Infrastructure.Data
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        { }

        public DbSet<Pagamento> Pagamentos { get; set; }
    }
}
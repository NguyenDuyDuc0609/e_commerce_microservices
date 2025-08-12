using Microsoft.EntityFrameworkCore;
using PaymentService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Infrastructure.Persistence
{
    public class PaymentContext(DbContextOptions<PaymentContext> options) : DbContext(options)
    {
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentEvents> PaymentEvents { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Payment>()
                .HasKey(p => p.PaymentId);
            modelBuilder.Entity<PaymentEvents>()
                .HasKey(pe => pe.PaymentEventId);
            modelBuilder.Entity<Payment>()
                .HasMany(p => p.PaymentEvents)
                .WithOne(pe => pe.Payment)
                .HasForeignKey(pe => pe.PaymentId)
                .OnDelete(DeleteBehavior.Cascade);

        }

    }
}

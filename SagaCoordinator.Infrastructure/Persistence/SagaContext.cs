using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using SagaCoordinator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Infrastructure.Persistence
{
    public class SagaContext : DbContext
    {
        public DbSet<SagaStatus> SagaStatuses { get; set; }
        public DbSet<OutboxState> OutboxStates { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }
        public SagaContext(DbContextOptions<SagaContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.Entity<SagaStatus>()
                .HasKey(s => s.CorrelationId);
        }
    }
}

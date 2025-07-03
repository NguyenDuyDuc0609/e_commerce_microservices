using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using SagaCoordinator.Domain.Constracts.SagaStates;
using SagaCoordinator.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Infrastructure.Persistence
{
    public class SagaContext(DbContextOptions<SagaContext> options) : DbContext(options)
    {
        public DbSet<SagaStatus> SagaStatuses { get; set; }
        public DbSet<RegisterSagaState> RegisterSagaStates { get; set; }
        public DbSet<ForgotPasswordSagaState> ForgotPasswordSagaStates { get; set; }
        public DbSet<OutboxState> OutboxStates { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.Entity<SagaStatus>()
                .HasKey(s => s.CorrelationId);
            modelBuilder.Entity<RegisterSagaState>()
                .HasKey(s => s.CorrelationId);
            modelBuilder.Entity<ForgotPasswordSagaState>()
                .HasKey(s => s.CorrelationId);
        }
    }
}

using Microsoft.EntityFrameworkCore.Storage;
using SagaCoordinator.Application.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SagaCoordinator.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SagaContext _context;
        private IDbContextTransaction? _transaction;
        public ISagaRepository SagaRepository { get; }

        public UnitOfWork(SagaContext context, ISagaRepository sagaRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            SagaRepository = sagaRepository ?? throw new ArgumentNullException(nameof(sagaRepository));
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();

            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

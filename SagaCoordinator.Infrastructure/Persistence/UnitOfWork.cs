using Microsoft.EntityFrameworkCore.Storage;
using SagaCoordinator.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SagaContext _context;
        private IDbContextTransaction? _transaction;
        public ISagaRedis? SagaRedis { get; }

        public ISagaRepository? SagaRepository { get; }
        public UnitOfWork(SagaContext context, ISagaRedis? sagaRedis, ISagaRepository? sagaRepository)
        {
            _context = context;
            SagaRedis = sagaRedis;
            SagaRepository = sagaRepository;
        }

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
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

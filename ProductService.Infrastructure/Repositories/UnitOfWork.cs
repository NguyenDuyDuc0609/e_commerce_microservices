using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ProductService.Application.Interfaces;
using ProductService.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories
{
    public class UnitOfWork(IRepository? repository, ProductContext productContext) : IUnitOfWork
    {
        public IRepository? Repository { get; private set; } = repository ?? throw new ArgumentNullException(nameof(repository));
        private readonly ProductContext _productContext = productContext ?? throw new ArgumentNullException(nameof(productContext));
        private IDbContextTransaction? _transaction;

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _productContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _transaction?.Dispose();
        }

        public async Task CommitAsync()
        {
            await _productContext.SaveChangesAsync();

            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task Rollback()
        {
            if(_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}

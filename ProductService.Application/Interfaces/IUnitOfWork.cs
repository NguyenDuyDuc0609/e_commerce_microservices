using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository? Repository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void Dispose();
        Task CommitAsync();
        Task Rollback();
    }
}

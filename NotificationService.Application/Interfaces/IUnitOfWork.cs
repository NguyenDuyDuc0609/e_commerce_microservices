using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Interfaces
{
    public interface IUnitOfWork
    {
        INotificationRepository? NotificationRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task CommitAsync();
        void BeginTransaction();
        Task RollbackAsync();
    }
}

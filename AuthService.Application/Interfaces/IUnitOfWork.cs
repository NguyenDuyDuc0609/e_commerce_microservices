using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository? UserRepository { get; }
        IRoleRepository? RoleRepository { get; }
        IUserRoleRepository? UserRoleRepository { get; }
        IUserSessionRepository? UserSessionRepository { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task CommitAsync();
        void BeginTransaction();
        Task RollbackAsync();
    }
}

using AuthService.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Persistence
{
    public class UnitOfWork(AuthDbContext context, IUserRepository? userRepository, IRoleRepository? roleRepository, IUserRoleRepository? userRoleRepository, IUserSessionRepository? userSessionRepository) : IUnitOfWork
    {
        private readonly AuthDbContext _context = context;
        private IDbContextTransaction? _transaction;
        public IUserRepository? UserRepository { get; } = userRepository;
        public IRoleRepository? RoleRepository { get; } = roleRepository;
        public IUserSessionRepository? UserSessionRepository { get; } = userSessionRepository;

        public IUserRoleRepository? UserRoleRepository { get; } = userRoleRepository;

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            if(_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if(_transaction != null)
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

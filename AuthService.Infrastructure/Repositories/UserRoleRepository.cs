using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AuthDbContext _context;
        public UserRoleRepository(AuthDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddUserRoleAsync(Guid userId, Guid roleId)
        {
            var UserRole = new UserRole(userId, roleId);
            await _context.UserRoles.AddAsync(UserRole);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

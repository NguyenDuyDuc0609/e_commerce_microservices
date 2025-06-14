using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AuthDbContext _auth;
        public RoleRepository(AuthDbContext auth)
        {
            _auth = auth;
        }
        public async Task<bool> AddAsync(Role entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Role>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Role?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> GetRole(string roleName)
        {
            var role = await _auth.Roles.Where(r => r.RoleName == roleName).FirstOrDefaultAsync();
            if (role == null)
            {
                throw new Exception($"Role with name {roleName} not found.");
            }
            return role.RoleId;
        }

        public Task<bool> UpdateAsync(Role entity)
        {
            throw new NotImplementedException();
        }
    }
}

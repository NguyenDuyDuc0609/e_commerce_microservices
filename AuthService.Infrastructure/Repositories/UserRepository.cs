using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _auth;
        public UserRepository(AuthDbContext auth)
        {
            _auth = auth;
        }
        public async Task<bool> AddAsync(User entity)
        {
            await _auth.Users.AddAsync(entity);
            return await _auth.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            await _auth.Users.AddAsync(user);
            return await _auth.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _auth.Users.Where( u => u.UserId == id).FirstOrDefaultAsync();
            user.MarkAsDeleted();
            _auth.Users.Update(user);
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            var user = await _auth.Users
                .Where(u => u.UserId == id)
                .FirstOrDefaultAsync();
            return user != null;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _auth.Users
                .Where(u => !u.IsDeleted)
                .ToListAsync();
            return users;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var users = await _auth.Users
                .Where(u => u.UserId == id)
                .FirstOrDefaultAsync();
            return users;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _auth.Users
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
            return user ?? throw new KeyNotFoundException("User not found with the provided email.");
        }

        public async Task<bool> UpdateAsync(User entity)
        {
            var user = await _auth.Users
                .Where(u => u.UserId == entity.UserId)
                .FirstOrDefaultAsync();
            if (user == null) return false;
            user.SetEmail(entity.Email);
            user.SetPhoneNumber(entity.PhoneNumber);
            user.SetAddress(entity.Address);
            _auth.Users.Update(user);
            return true;
        }

        public async Task<bool> UserExistsAsync(string username, string email)
        {
            var user = await _auth.Users
                .Where(u => (u.Username == username || u.Email == email))
                .FirstOrDefaultAsync();
            if(user == null)
            {
                return true;
            }
            return false;
        }
    }
}

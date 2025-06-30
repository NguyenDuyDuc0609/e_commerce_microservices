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
    public class UserRepository(AuthDbContext auth) : IUserRepository
    {
        private readonly AuthDbContext _auth = auth;

        public async Task<bool> AddAsync(User entity)
        {
            var entry = await _auth.Users.AddAsync(entity);
            return entry.State == EntityState.Added;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            var entry = await _auth.Users.AddAsync(user);
            return entry.State == EntityState.Added;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _auth.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
            {
                return false;
            }

            user.MarkAsDeleted();
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
                .Where(u => (u.Username == username || u.Email == email) && u.IsActive)
                .FirstOrDefaultAsync();
            return user != null;
        }

        public async Task<(User? user, string? message)> VerifyLogin(string username, string password)
        {
            var user = await _auth.Users.Where(u => u.Username == username)
                .Include(u => u.UserRoles)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync();
            if (user == null)
                return (null, "User not found.");
            if (!user.IsActive)
                return (null, "User is not active. Please activate your account.");
            if(user.VerifyPassword(password))
                return (user, "Login succes");
            return (null, "Invalid username or password.");
        }
    }
}

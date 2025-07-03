using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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

        public async Task<(bool? isSuccess, string? message)> ChangePassword(Guid userId, string oldPassword, string newPassword)
        {
            var user = await _auth.Users.Where(u => u.UserId == userId && u.IsActive)
                .FirstOrDefaultAsync();
            if (user == null)
                return (false, "User not found or inactive.");
            var result = user.ChangePassword(oldPassword, newPassword);
            if (result)
            {
                _auth.Update(user);
                return (true, "Password changed successfully.");
            }
            return (false, "Old password is incorrect.");
        }

        public async Task<(bool isSucess, string token)> CreateToken(string email)
        {
            var user = await _auth.Users
                .Where(u => u.Email == email && u.IsActive)
                .FirstOrDefaultAsync();
            if (user == null) return (false, "User not found or inactive.");
            var token = new PasswordResetTokens
            {
                UserId = user.UserId,
            };
            await _auth.PasswordResetTokens.AddAsync(token);
            return (true, token.Token);

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

        public async Task<(bool? isSucces, string? message)> ResetPassword(string token, string newPassword)
        {
            var passwordResetToken = await _auth.PasswordResetTokens.Where(t => t.Token == token)
                .Include(t => t.User)
                .FirstOrDefaultAsync();
            if(passwordResetToken == null) return (false, "Token not found");
            if (passwordResetToken.ExpiresAt < DateTimeOffset.UtcNow)
            {
                passwordResetToken.MarkAsExpired();
                passwordResetToken.MarkAsDeleted();
                _auth.PasswordResetTokens.Update(passwordResetToken);
                return (false, "Token is expired.");
            }
            if(passwordResetToken.User == null) return (false, "User not found for the provided token.");
            passwordResetToken.User.SetPasswordHash(newPassword);
            passwordResetToken.MarkAsExpired();
            passwordResetToken.MarkAsDeleted();
            _auth.PasswordResetTokens.Update(passwordResetToken);
            return (true, "Password reset successfully.");

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

        public async Task<bool> VerifyEmail(string email)
        {
            var user = await _auth.Users.Where(e => e.HashEmailVerification == email)
                .FirstOrDefaultAsync();
            if (user == null)
                return false;
            user.Activate();
            _auth.Users.Update(user);
            return true;
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

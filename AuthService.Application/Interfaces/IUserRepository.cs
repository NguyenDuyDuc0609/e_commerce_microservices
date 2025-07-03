using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Entities;
using AuthService.Application.Features.Users.Commands;
namespace AuthService.Application.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<bool> UserExistsAsync(string username, string email);
        Task<bool> AddUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<(User? user, string? message)> VerifyLogin(string username, string password);
        Task<bool> VerifyEmail(string email);
        Task<(bool? isSuccess, string? message)> ChangePassword(Guid userId, string oldPassword, string newPassword);
        Task<(bool? isSucces, string? message)> ResetPassword(string token, string newPassword);
        Task<(bool isSucess, string token)> CreateToken(string email);
    }
}

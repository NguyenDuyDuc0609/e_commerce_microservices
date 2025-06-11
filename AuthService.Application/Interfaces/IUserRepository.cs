using AuthService.Application.Features.Users.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.Entities;
namespace AuthService.Application.Interfaces
{
    public interface IUserRepository
    {
        public Task<bool> UserExistsAsync(string username, string email);
        Task<bool> AddUserAsync(RegisterRequest registerRequest);
        Task<User> GetUserByEmailAsync(string email);
        
    }
}

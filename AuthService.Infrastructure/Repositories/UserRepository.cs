using AuthService.Application.Features.Users.Register;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<bool> AddUserAsync(RegisterRequest registerRequest)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UserExistsAsync(string username, string email)
        {
            throw new NotImplementedException();
        }
    }
}

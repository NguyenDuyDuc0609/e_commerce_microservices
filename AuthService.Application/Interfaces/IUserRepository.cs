﻿using System;
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
    }
}

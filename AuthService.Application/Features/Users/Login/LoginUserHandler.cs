using AuthService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Users.Login
{
    public class LoginUserHandler
    {
        private readonly IUserRepository _userRepository;
        public LoginUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

    }
}

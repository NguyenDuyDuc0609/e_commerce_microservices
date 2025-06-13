using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Users.Login
{
    public class LoginResponse
    {
        public object? DataResponse { get; set; }
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}

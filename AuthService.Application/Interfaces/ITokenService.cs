using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface ITokenService
    {
        public string GenerateJWT(User user, string role);
        public string GenerateRefreshToken();
        public ClaimsPrincipal? GetClaimsPrincipalToken(string? token);
    }
}

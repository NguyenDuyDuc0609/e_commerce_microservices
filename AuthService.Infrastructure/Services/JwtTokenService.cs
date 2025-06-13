using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Services
{
    public class JwtTokenService : ITokenService
    {
        public string GenerateJWT(User user)
        {
            throw new NotImplementedException();
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? GetClaimsPrincipalToken(string? token)
        {
            throw new NotImplementedException();
        }
    }
}

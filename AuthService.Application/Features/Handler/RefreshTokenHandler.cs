using AuthService.Application.Features.Users.Commands;
using AuthService.Application.Features.Users.Dtos;
using AuthService.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Handler
{
    public class RefreshTokenHandler(IUnitOfWork unitOfWork, IAuthRedisCacheService authRedisCacheService, ITokenService tokenService) : IRequestHandler<RefreshTokenCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAuthRedisCacheService _authRedisCacheService = authRedisCacheService;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<UserDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.RefreshToken)) return new UserDto { IsSuccess = false, Message = "Token or RefreshToken cannot be null or empty." };
            var principal = _tokenService.GetClaimsPrincipalToken(request.Token);
            if(principal?.Identity?.Name is  null) return new UserDto { IsSuccess = false, Message = "Invalid token." };
            var userIdClaim = principal?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId)) return new UserDto { IsSuccess = false, Message = "Invalid user ID in token." };
            var refreshToken = await _authRedisCacheService.GetTokenAsync<string>(userId.ToString());
            if (refreshToken == null) return new UserDto { IsSuccess = false, Message = "Refresh token is expired, please login again" };
            if (refreshToken != request.RefreshToken) return new UserDto { IsSuccess = false, Message = "Invalid refresh token." };
            var user = await _unitOfWork.UserRepository!.GetByIdAsync(userId);
            if (user == null) return new UserDto { IsSuccess = false, Message = "User not found." };
            var role = principal?.FindFirst(ClaimTypes.Role)?.Value;
            var token = _tokenService.GenerateJWT(user, role!);
            return new UserDto
            {
                Message = "Token refreshed successfully.",
                IsSuccess = true,
                Data = new
                {
                    Token = token,
                }
            };
        }
    }
}

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
    public class LogoutHandler(IUnitOfWork unitOfWork, ITokenService tokenService, IAuthRedisCacheService authRedisCacheService) : IRequestHandler<LogoutCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ITokenService _tokenService = tokenService;
        private readonly IAuthRedisCacheService _authRedisCacheService = authRedisCacheService ;
        public async Task<UserDto> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var principal = _tokenService.GetClaimsPrincipalToken(request.Token);
                if (principal?.Identity?.Name is null) return new UserDto { IsSuccess = false, Message = "Invalid token." };
                var userIdClaim = principal?.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId)) return new UserDto { IsSuccess = false, Message = "Invalid user ID in token." };
                var result = await _unitOfWork.UserSessionRepository!.Logout(userId, request.Token, request.DeviceInfo, request.IpAddress);
                if(!result)
                {
                    return new UserDto { IsSuccess = false, Message = "Logout failed. Device not found or already logged out." };
                }
                var sessionId = Guid.Parse(principal!.FindFirst("SessionId")!.Value);
                await _authRedisCacheService.RemoveTokenAsync(sessionId.ToString());
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return new UserDto
                {
                    IsSuccess = true,
                    Message = "Logout successful."
                };
            }
            catch (Exception ex)
            {
                return new UserDto
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}

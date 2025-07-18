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
    public class DeleteDeviceHandler(IUnitOfWork unitOfWork, ITokenService tokenService, IAuthRedisCacheService authRedisCacheService) : IRequestHandler<DeleteDeviceCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ITokenService _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        private readonly IAuthRedisCacheService _authRedisCacheService = authRedisCacheService ?? throw new ArgumentNullException(nameof(authRedisCacheService));

        public async Task<UserDto> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var principal = _tokenService.GetClaimsPrincipalToken(request.Token);
                if (principal?.Identity?.Name is null) return new UserDto { IsSuccess = false, Message = "Invalid token." };
                var userIdClaim = principal?.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId)) return new UserDto { IsSuccess = false, Message = "Invalid user ID in token." };
                var result = await _unitOfWork.UserSessionRepository!.DeleteDevice(userId, request.DeviceInfo);
                if (!result)
                {
                    return new UserDto { IsSuccess = false, Message = "Device not found or already logged out." };
                }
                var sessionId = Guid.Parse(principal!.FindFirst("SessionId")!.Value);
                await _authRedisCacheService.RemoveTokenAsync(sessionId.ToString());
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return new UserDto
                {
                    IsSuccess = true,
                    Message = "Device deleted successfully."
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

using AuthService.Application.Features.Users.Commands;
using AuthService.Application.Features.Users.Dtos;
using AuthService.Application.Interfaces;
using MassTransit;
using MassTransit.NewIdProviders;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Handler
{
    public class LoginUserHandler(IUnitOfWork unitOfWork, ITokenService tokenService, IPublishEndpoint publishEndpoint) : IRequestHandler<LoginUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<UserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Username == null || request.Password == null) return new UserDto { IsSuccess = false, Message = "Username and password cannot be null."};

                var user = await _unitOfWork.UserRepository!.VerifyLogin(request.Username, request.Password);

                if(user.user == null || !user.user.IsActive || user.message != null) return new UserDto{ IsSuccess = false, Message = user.message ?? "Invalid username or password."};

                string? roleName = user.user.UserRoles?.FirstOrDefault()?.Role?.RoleName;

                if (roleName == null || roleName == string.Empty) return new UserDto { IsSuccess = false, Message = "User role not found."};
                var refreshtoken = _tokenService.GenerateRefreshToken();
                
                var result = await _unitOfWork.UserSessionRepository!.NewLoginDevice(user.user.UserId, request.DeviceInfor, request.IpAddress, refreshtoken);
                var token = _tokenService.GenerateJWT(user.user, roleName, result);
                if (result == Guid.Empty) return new UserDto { IsSuccess = false, Message = "Failed to create new session for user." };
                await publishEndpoint.Publish(new UpdateCache
                {
                    SessionId = result,
                    RefreshToken = refreshtoken
                }, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return new UserDto
                {
                    Data = new LoginResponse
                    {
                        Username = user.user.Username,
                        Token = token,
                        RefreshToken = refreshtoken,
                        PhoneNumber = user.user.PhoneNumber,
                        Address = user.user.Address,
                        Role = roleName,
                    },
                    IsSuccess = true,
                    Message = "Login successful.",
                };
            }
            catch (Exception ex)
            {
                return new UserDto{ IsSuccess = false, Message = ex.Message };
            }
        }
    }
}

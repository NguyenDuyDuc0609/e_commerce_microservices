using AuthService.Application.Features.Users.Commands;
using AuthService.Application.Features.Users.Dtos;
using AuthService.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Handler
{
    public class ChangePasswordHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangePasswordCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UserDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if(!Guid.TryParse(request.UserId, out var userId))
                return new UserDto
                {
                    IsSuccess = false,
                    Message = "Invalid UserId format."
                };
            try
            {
                var (isSuccess, message) = await _unitOfWork.UserRepository!.ChangePassword(userId, request.OldPassword, request.NewPassword);
                if(isSuccess == false)
                {
                    return new UserDto
                    {
                        IsSuccess = false,
                        Message = message
                    };
                }
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return new UserDto
                {
                    IsSuccess = true,
                    Message = "Password changed successfully."
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

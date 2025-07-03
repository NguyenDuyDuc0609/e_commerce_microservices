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
    public class ForgotPasswordHandler(IUnitOfWork unitOfWork) : IRequestHandler<PasswordForgotCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UserDto> Handle(PasswordForgotCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var (isSuccess, message) = await _unitOfWork.UserRepository!.CreateToken(request.Email);
                if (!isSuccess)
                {
                    return new UserDto
                    {
                        IsSuccess = false,
                        Message = message ?? "Failed to create token for password reset."
                    };
                }
                return new UserDto
                {
                    IsSuccess = true,
                    Message = message
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

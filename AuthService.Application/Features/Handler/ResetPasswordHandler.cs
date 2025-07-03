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
    public class ResetPasswordHandler(IUnitOfWork unitOfWork) : IRequestHandler<ResetPasswordCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UserDto> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var (isSucces, message) = await _unitOfWork.UserRepository!.ResetPassword(request.Token, request.NewPassword);
                if (isSucces == false)
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
                    Message = "Password reset successfully."
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

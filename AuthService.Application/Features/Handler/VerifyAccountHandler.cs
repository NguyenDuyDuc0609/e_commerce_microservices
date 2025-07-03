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
    public class VerifyAccountHandler(IUnitOfWork unitOfWork) : IRequestHandler<VerifyAccountCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UserDto> Handle(VerifyAccountCommand request, CancellationToken cancellationToken)
        {
            if(request.HashEmail == null || request.HashEmail == string.Empty)
                return new UserDto { IsSuccess = false, Message = "Email verification hash cannot be null or empty." };

            var result = _unitOfWork.UserRepository!.VerifyEmail(request.HashEmail);
            if (result.Result)
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return (new UserDto { IsSuccess = true, Message = "Email verified successfully." });
            }
            else
                return new UserDto { IsSuccess = false, Message = "Email verification failed." };
        }
    }
}

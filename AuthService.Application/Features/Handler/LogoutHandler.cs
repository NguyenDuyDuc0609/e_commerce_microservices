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
    public class LogoutHandler(IUnitOfWork unitOfWork) : IRequestHandler<LogoutCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public async Task<UserDto> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
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

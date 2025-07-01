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

        public Task<UserDto> Handle(VerifyAccountCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

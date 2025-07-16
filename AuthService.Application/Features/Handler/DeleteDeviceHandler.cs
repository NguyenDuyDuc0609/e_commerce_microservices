using AuthService.Application.Features.Users.Commands;
using AuthService.Application.Features.Users.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Handler
{
    public class DeleteDeviceHandler : IRequestHandler<DeleteDeviceCommand, UserDto>
    {
        public Task<UserDto> Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

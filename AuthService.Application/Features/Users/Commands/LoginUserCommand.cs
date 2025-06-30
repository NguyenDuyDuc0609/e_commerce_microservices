using AuthService.Application.Features.Users.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Users.Commands
{
    public record LoginUserCommand(string Username, string Password, string DeviceInfor, string IpAddress) : IRequest<UserDto>;
}

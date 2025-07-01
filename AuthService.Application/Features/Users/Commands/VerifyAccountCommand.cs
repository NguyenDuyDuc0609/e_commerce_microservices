using AuthService.Application.Features.Users.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Users.Commands
{
    public record VerifyAccountCommand(string HashEmail) : IRequest<UserDto>;
}

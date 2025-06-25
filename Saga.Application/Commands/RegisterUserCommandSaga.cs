using MediatR;
using SagaCoordinator.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Application.Commands
{
    public record RegisterUserCommandSaga(string? Username, string? Email, string? PasswordHash, string? PhoneNumber, string? Address) : IRequest<ModelResult>;
}

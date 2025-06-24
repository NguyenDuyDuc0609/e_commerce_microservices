using MassTransit;
using MediatR;
using SagaCoordinator.Application.Commands;
using SagaCoordinator.Application.Dtos;
using SagaCoordinator.Domain.Constracts.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Application.Handlers
{
    public class RegisterSagaHanlder : IRequestHandler<RegisterUserCommandSaga, MessageResult>
    {
        private readonly IRequestClient<RegisterUserCommand> _requestClient;
        public RegisterSagaHanlder(IRequestClient<RegisterUserCommand> requestClient)
        {
            _requestClient = requestClient;
        }
        public async Task<MessageResult> Handle(RegisterUserCommandSaga request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new RegisterUserCommand
                {
                    CorrelationId = Guid.NewGuid(),
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = request.PasswordHash,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address
                };
                var response = await _requestClient.GetResponse<MessageResult>(command, cancellationToken: cancellationToken);
                return response.Message;
            }
            catch (Exception ex)
            {
                var notification = new MessageResult
                {
                    Message = ex.Message,
                    IsSuccess = false
                };
                return notification;
            }
            throw new NotImplementedException();
        }
    }
}

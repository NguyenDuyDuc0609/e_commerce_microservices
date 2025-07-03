using MassTransit;
using MediatR;
using SagaCoordinator.Application.Commands;
using SagaCoordinator.Application.Dtos;
using SagaCoordinator.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Application.Handlers
{
    public class ForgotPasswordSagaHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint) : IRequestHandler<ForgotPasswordSagaCommand, ModelResult>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        public Task<ModelResult> Handle(ForgotPasswordSagaCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

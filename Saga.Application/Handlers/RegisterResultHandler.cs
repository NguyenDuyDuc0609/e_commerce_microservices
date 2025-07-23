using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
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
    public class RegisterResultHandler(IUnitOfWork unitOfWork) : IRequestHandler<RegisterResultCommand, ModelResult>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public async Task<ModelResult> Handle(RegisterResultCommand request, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.CorrelationId, out var correlationId))
            {
                return new ModelResult
                {
                    Message = "Invalid correlation ID format.",
                    CorrelationId = correlationId,
                };
            }
            try
            {
                var result = await _unitOfWork.SagaRepository!.GetSagaStatus(correlationId);
                return new ModelResult
                {
                    Message = result.Value.ToString(),
                    CorrelationId = correlationId,
                };
            }
            catch (Exception ex)
            {
                return new ModelResult
                {
                    Message = ex.Message,
                    CorrelationId = correlationId,
                };
            }
        }
    }
}

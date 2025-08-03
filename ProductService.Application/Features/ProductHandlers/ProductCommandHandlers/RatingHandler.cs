using MediatR;
using ProductService.Application.Features.Dtos;
using ProductService.Application.Features.Products.ProductCommands;
using ProductService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.ProductHandlers.ProductCommandHandlers
{
    public class RatingHandler(IUnitOfWork unitOfWork) : IRequestHandler<RatingCommand, CommandDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public Task<CommandDto> Handle(RatingCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

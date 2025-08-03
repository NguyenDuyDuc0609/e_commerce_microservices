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
    public class AddSKUHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddSKUCommand, CommandDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        public Task<CommandDto> Handle(AddSKUCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

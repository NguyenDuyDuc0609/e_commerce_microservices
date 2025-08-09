using MediatR;
using ProductService.Application.Features.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Products.ProductCommands
{
    public record UpdateProductCommand(UpdateProductDto UpdateProductDto) : IRequest<CommandDto>;
}

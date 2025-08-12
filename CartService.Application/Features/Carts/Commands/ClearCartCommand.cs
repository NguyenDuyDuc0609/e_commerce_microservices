using CartService.Application.Features.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Features.Carts.Commands
{
    public record ClearCartCommand : IRequest<CartServiceResult>;
}

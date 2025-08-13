using CartService.Application.Features.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Features.Carts.Queries
{
    public record CartQuery(string Token, int PageNumber, int PageSize) : IRequest<CartServiceResult>;
}

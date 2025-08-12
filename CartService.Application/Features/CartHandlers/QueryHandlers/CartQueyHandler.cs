using CartService.Application.Features.Carts.Queries;
using CartService.Application.Features.Dtos;
using CartService.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Features.CartHandlers.QueryHandlers
{
    public class CartQueyHandler(IQueryService queryService, ILogger<CartQueyHandler> logger) : IRequestHandler<CartQuery, CartServiceResult>
    {
        private readonly IQueryService _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
        private readonly ILogger<CartQueyHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<CartServiceResult> Handle(CartQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

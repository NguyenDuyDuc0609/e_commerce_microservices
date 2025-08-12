using CartService.Application.Features.Carts.Commands;
using CartService.Application.Features.Dtos;
using CartService.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Features.CartHandlers.CommandHandlers
{
    public class ClearCartHandler(ICommandService commandService, ILogger<ClearCartHandler> logger) : IRequestHandler<ClearCartCommand, CartServiceResult>
    {
        private readonly ICommandService _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        private readonly ILogger<ClearCartHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<CartServiceResult> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

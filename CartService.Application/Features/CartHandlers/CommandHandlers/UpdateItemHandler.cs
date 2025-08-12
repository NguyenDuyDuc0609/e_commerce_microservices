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
    public class UpdateItemHandler(ICommandService commandService, ILogger<UpdateItemHandler> logger) : IRequestHandler<UpdateItemCommand, CartServiceResult>
    {
        private readonly ICommandService _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        private readonly ILogger<UpdateItemHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<CartServiceResult> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

﻿using AuthService.Application.Features.Users.Commands;
using AuthService.Application.Features.Users.Dtos;
using AuthService.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Handler
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        public LoginUserHandler(IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }
        public async Task<UserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.BeginTransaction();
            throw new NotImplementedException("LoginUserHandler is not implemented yet. Please implement the logic to handle user login.");
        }
    }
}

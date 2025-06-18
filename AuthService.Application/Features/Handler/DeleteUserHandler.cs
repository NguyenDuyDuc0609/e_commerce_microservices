using AuthService.Application.Features.Users.Commands;
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
    public class DeleteUserHandler : IRequestHandler<DeleteCommad, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<UserDto> Handle(DeleteCommad request, CancellationToken cancellationToken)
        {
            _unitOfWork.BeginTransaction();
            try
            {

            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackAsync();
                return Task.FromResult(new UserDto
                {
                    IsSuccess = false,
                    Message = ex.Message
                });
            }
            throw new NotImplementedException();
        }
    }
}

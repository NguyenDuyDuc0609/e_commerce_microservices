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
        public async Task<UserDto> Handle(DeleteCommad request, CancellationToken cancellationToken)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var result = await _unitOfWork.UserRepository.DeleteAsync(request.UserId);
                if(!result)
                {
                    return new UserDto
                    {
                        IsSuccess = false,
                        Message = "User deletion failed or user does not exist."
                    };
                }
                await _unitOfWork.CommitAsync();
                return new UserDto
                {
                    IsSuccess = true,
                    Message = "User deleted successfully."
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return new UserDto
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
            throw new NotImplementedException();
        }
    }
}

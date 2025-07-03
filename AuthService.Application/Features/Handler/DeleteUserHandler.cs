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
    public class DeleteUserHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteCommad, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UserDto> Handle(DeleteCommad request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unitOfWork.UserRepository!.DeleteAsync(request.UserId);
                if(!result)
                {
                    return new UserDto
                    {
                        IsSuccess = false,
                        Message = "User deletion failed or user does not exist."
                    };
                }
                return new UserDto
                {
                    IsSuccess = true,
                    Message = "User deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new UserDto
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}

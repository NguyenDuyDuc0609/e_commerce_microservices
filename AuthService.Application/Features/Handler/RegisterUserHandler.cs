using AuthService.Application.Features.Users.Commands;
using AuthService.Application.Features.Users.Dtos;
using AuthService.Application.Helper;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Handler
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommands, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegisterUserHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<UserDto> Handle(RegisterUserCommands request, CancellationToken cancellationToken)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var validateEmail = ValidateEmail.IsValidEmail(request.Email);
                if(!validateEmail)
                {
                    return new UserDto
                    {
                        IsSuccess = false,
                        Message = "Invalid email format."
                    };
                }
                var checkExits = await _unitOfWork.UserRepository.UserExistsAsync(request.Username, request.Email);
                if (!checkExits)
                {
                    return new UserDto
                    {
                        IsSuccess = false,
                        Message = "User already exists with the provided username or email."
                    };
                }
                var user = new User(request.Username, request.Email, request.PasswordHash, request.PhoneNumber, request.Address);
                var result = await _unitOfWork.UserRepository.AddUserAsync(user);
                var role = await _unitOfWork.RoleRepository.GetRole("User");
                var userRole = await _unitOfWork.UserRoleRepository.AddUserRoleAsync(user.UserId, role);
                if (userRole == null)
                {
                    return new UserDto
                    {
                        IsSuccess = false,
                        Message = "Failed to assign role to user."
                    };
                }
                if (result == false)
                    return new UserDto
                    {
                        IsSuccess = false,
                        Message = "Failed to register user. Please try again."
                    };
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitAsync();
                return new UserDto
                {
                    IsSuccess = true,
                    Message = "User registered successfully.",
                    UserId = user.UserId,
                    HashEmail = user.HashEmailVerification
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();

                Console.WriteLine("Error message: " + ex.Message);
                Console.WriteLine("Stack trace: " + ex.StackTrace);

                return new UserDto
                {
                    IsSuccess = false,
                    Message = $"An error occurred while registering the user: {ex.Message}"
                };
            }
        }
    }
}

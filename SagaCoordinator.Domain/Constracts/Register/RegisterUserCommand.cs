using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Domain.Constracts.Register
{
    public class RegisterUserCommand
    {
        public Guid CorrelationId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public RegisterUserCommand(Guid correlationId, string? username, string? email, string? passwordHash, string? phoneNumber, string? address)
        {
            CorrelationId = correlationId;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            PhoneNumber = phoneNumber;
            Address = address;
        }
        public RegisterUserCommand()
        {
        }
    }
}

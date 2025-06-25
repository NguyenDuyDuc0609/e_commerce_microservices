using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterConstracts.Commands
{
    public record RegisterUserCommand
    {
        public Guid CorrelationId { get; init; }
        public string? Username { get; init; }
        public string? Email { get; init; }
        public string? PasswordHash { get; init; }
        public string? PhoneNumber { get; init; }
        public string? Address { get; init; }
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

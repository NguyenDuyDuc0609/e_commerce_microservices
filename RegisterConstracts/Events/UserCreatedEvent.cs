using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterConstracts.Events
{
    public record UserCreatedEvent
    {
        public Guid CorrelationId { get; init; }
        public Guid UserId { get; init; }
        public string? Username { get; init; }
        public string? Email { get; init; }
        public string? HashEmail { get; init; }
        public string? PhoneNumber { get; init; }
        public string? Address { get; init; }
        public UserCreatedEvent(Guid correlationId, Guid userId, string? username, string? email, string? hashEmail, string? phoneNumber, string? address)
        {
            CorrelationId = correlationId;
            UserId = userId;
            Username = username;
            Email = email;
            HashEmail = hashEmail;
            PhoneNumber = phoneNumber;
            Address = address;
        }
        public UserCreatedEvent()
        {
        }
    }
}

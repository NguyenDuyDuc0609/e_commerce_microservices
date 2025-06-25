using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterConstracts.Commands
{
    public record NotificationRegisterCommand
    {
        public Guid CorrelationId { get; init; }
        public Guid UserId { get; init; }
        public string? Username { get; init; }
        public string? Email { get; init; }
        public string? HashEmail { get; init; }
        public NotificationRegisterCommand(Guid correlationId, Guid userId, string? username, string? email, string? hashEmail)
        {
            CorrelationId = correlationId;
            UserId = userId;
            Username = username;
            Email = email;
            HashEmail = hashEmail;
        }
        public NotificationRegisterCommand()
        {
        }
    }
}

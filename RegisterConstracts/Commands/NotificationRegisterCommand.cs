using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterConstracts.Commands
{
    public class NotificationRegisterCommand
    {
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? HashEmail { get; set; }
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

using NotificationService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Features.Dtos
{
    public class NotificationMessage
    {
        public NotificationType Type { get; set; }
        public string? UserName { get; set; } 
        public Guid UserId { get; set; }
        public string? HashEmail { get; set; }
        public string? Email { get; set; }
        public NotificationMessage() { }
        public NotificationMessage(NotificationType type, string? userName, Guid userId, string? hashEmail, string? email)
        {
            Type = type;
            UserName = userName;
            UserId = userId;
            HashEmail = hashEmail;
            Email = email;
        }
    }
}

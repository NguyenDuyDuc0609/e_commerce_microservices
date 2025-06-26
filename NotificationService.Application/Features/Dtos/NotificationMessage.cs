using NotificationService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Features.Dtos
{
    public class NotificationMessage(NotificationType type, string? userName, Guid userId, string hashEmail, string email)
    {
        public NotificationType Type { get; set; } = type;
        public string? UserName { get; set; } = userName;
        public Guid UserId { get; set; } = userId;
        public string HashEmail { get; set; } = hashEmail;
        public string Email { get; set; } = email;
    }
}

using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Interfaces
{
    public interface INotificationRepository
    {
        Task<bool> AddNotificationAsync(Guid? userId, string recipient, string subject, string body, NotificationType type);
        Task<IEnumerable<NotificationLog>> GetNotificationLogsAsync(Guid userId, int pageNumber, int pageSize);
    }
}

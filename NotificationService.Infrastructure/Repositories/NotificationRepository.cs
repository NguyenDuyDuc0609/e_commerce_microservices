using Microsoft.EntityFrameworkCore;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Entities;
using NotificationService.Domain.Enums;
using NotificationService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Repositories
{
    public class NotificationRepository(NotificationContext context) : INotificationRepository
    {
        private readonly NotificationContext _context = context;

        public async Task<bool> AddNotificationAsync(Guid? userId, string recipient, string subject, string body, NotificationType type)
        {
            if (userId is null) return false;
            var notification = new NotificationLog(userId.Value, recipient, subject, body, type);
            var entry = await _context.NotificationLogs.AddAsync(notification);
            return entry.State == EntityState.Added;
        }

        public Task<IEnumerable<NotificationLog>> GetNotificationLogsAsync(Guid userId, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}

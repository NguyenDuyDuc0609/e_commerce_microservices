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
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationContext _context;
        public NotificationRepository(NotificationContext context)
        {
            _context = context;
        }
        public async Task<bool> AddNotificationAsync(Guid userId, string recipient, string subject, string body, NotificationType type)
        {
            var notification = new NotificationLog(userId, recipient, subject, body, type);
            var result = await _context.NotificationLogs.AddAsync(notification);
            if (result.State == Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }


        public async Task<IEnumerable<NotificationLog>> GetNotificationLogsAsync(Guid userId, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}

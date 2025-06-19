using NotificationService.Application.Features.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Interfaces
{
    public interface INotificationStrategy
    {
        Task<NotificationResult> SendAsync(NotificationMessage message);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Interfaces
{
    public interface IEmailSender
    {
        Task<(bool isSuccess, string message)> SendEmailAsync(string to, string subject, string hashEmail);
    }
}

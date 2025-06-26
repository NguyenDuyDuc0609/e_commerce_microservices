using NotificationService.Application.Features.Dtos;
using NotificationService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.EmailStrategy
{
    public class RegisterSendMail(IEmailSender emailSender) : INotificationStrategy
    {
        private readonly IEmailSender _emailSender = emailSender;

        public async Task<NotificationResult> SendAsync(NotificationMessage message)
        {
            var result = await _emailSender.SendEmailAsync(message.Email, message.UserName, message.HashEmail);
            if (result.isSuccess)
            {
                return new NotificationResult
                {
                    IsSuccess = true,
                    Message = result.message,
                };
            }
            return new NotificationResult
            {
                IsSuccess = false,
                Message = result.message,
            };
        }
    }
}

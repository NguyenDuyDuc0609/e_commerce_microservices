using NotificationService.Application.Features.Dtos;
using NotificationService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.EmailStrategy
{
    public class RegisterSendMail : INotificationStrategy
    {
        private readonly IEmailSender _emailSender;
        public RegisterSendMail(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        public async Task<NotificationResult> SendAsync(NotificationMessage message)
        {
            var result = await _emailSender.SendEmailAsync(message.Email, message.UserName, "Click here to virify your acccount");
            if (result)
            {
                return new NotificationResult
                {
                    IsSuccess = true,
                    Message = "Email sent successfully.",
                };
            }
            return new NotificationResult
            {
                IsSuccess = false,
                Message = "Failed to send email.",
            };
        }
    }
}

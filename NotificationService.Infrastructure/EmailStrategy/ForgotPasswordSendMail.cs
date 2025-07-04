using NotificationService.Application.Features.Dtos;
using NotificationService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.EmailStrategy
{
    public class ForgotPasswordSendMail(IEmailSender emailSender) : INotificationStrategy
    {
        private readonly IEmailSender _emailSender = emailSender;

        public async Task<NotificationResult> SendAsync(NotificationMessage message)
        {
            var body = $"This is your token to change password: {message.HashEmail} ";
            var result = await _emailSender.SendEmailAsync(message.Email, "Forgot Password", body);
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

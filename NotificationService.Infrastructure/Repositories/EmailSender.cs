using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NotificationService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Repositories
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var message = new MimeKit.MimeMessage();
                message.From.Add(new MimeKit.MailboxAddress("Notification Service", _configuration["EmailSettings:From"]));
                message.To.Add(new MimeKit.MailboxAddress(to, to));
                message.Subject = subject;
                message.Body = new MimeKit.TextPart("html")
                {
                    Text = body
                };
                using var smtp = new SmtpClient();
                smtp.Connect(_configuration["EmailConfig:smtp"], int.Parse(_configuration["EmailConfig:SmtpPort"]), SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration["EmailConfig:Email"], _configuration["EmailConfig:EmailPassword"]);
                smtp.Send(message);
                smtp.Disconnect(true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

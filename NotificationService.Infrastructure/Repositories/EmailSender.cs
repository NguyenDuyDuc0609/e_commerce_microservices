using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using NotificationService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Repositories
{
    public class EmailSender(IConfiguration configuration) : IEmailSender
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<(bool isSuccess, string message)> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var message = new MimeKit.MimeMessage();
                message.From.Add(MailboxAddress.Parse(_configuration["EmailConfig:Email"]));
                message.To.Add(MailboxAddress.Parse(to));
                message.Subject = subject;
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = body
                };
                using var smtp = new SmtpClient();
                smtp.Connect(_configuration["EmailConfig:smtp"], int.Parse(_configuration["EmailConfig:SmtpPort"]!), SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration["EmailConfig:Email"], _configuration["EmailConfig:EmailPassword"]);
                smtp.Send(message);
                smtp.Disconnect(true);
                return (true, "Email sent successfully.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}

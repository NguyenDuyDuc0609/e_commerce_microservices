using NotificationService.Application.Interfaces;
using NotificationService.Domain.Enums;
using NotificationService.Infrastructure.EmailStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Services
{
    public class NotificationStrategySelector(RegisterSendMail registerSendmail, ForgotPasswordSendMail forgotPasswordSendMail) : INotificationStrategySelector
    {
        private readonly RegisterSendMail _registerSendmail = registerSendmail;
        private readonly ForgotPasswordSendMail _forgotPasswordSendMail = forgotPasswordSendMail;

        public INotificationStrategy GetStrategy(NotificationType type)
        {
            return type switch
            {
                NotificationType.RegisterEmail => _registerSendmail,
                NotificationType.ForgotPasswordEmail => _forgotPasswordSendMail,
                _ => throw new NotImplementedException($"Strategy for {type} is not implemented.")
            };
        }
    } 
}

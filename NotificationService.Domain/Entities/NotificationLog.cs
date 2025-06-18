using NotificationService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Domain.Entities
{
    public class NotificationLog
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Recipient { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public NotificationType Type { get; private set; }
        public NotificationStatus Status { get; private set; }
        public DateTime CreatedAt { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTimeOffset ModifyDate { get; private set; }
        public NotificationLog() { }
        public NotificationLog(Guid userId, string recipient, string subject, string body, NotificationType type)
        {
            Id = Guid.NewGuid();
            Recipient = recipient;
            Subject = subject;
            Body = body;
            Type = type;
            Status = NotificationStatus.Pending;
            CreatedAt = DateTime.UtcNow;
            ModifyDate = DateTimeOffset.UtcNow;
            UserId = userId;
        }
        public void SetStatus(NotificationStatus status, string? errorMessage = null)
        {
            Status = status;
            ErrorMessage = errorMessage;
            ModifyDate = DateTimeOffset.UtcNow;
        }
        public void SetRecipient(string recipient)
        {
            if (string.IsNullOrWhiteSpace(recipient))
            {
                throw new ArgumentException("Recipient cannot be null or empty.", nameof(recipient));
            }
            Recipient = recipient;
        }
        public void SetSubject(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentException("Subject cannot be null or empty.", nameof(subject));
            }
            Subject = subject;
        }
        public void SetBody(string body)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                throw new ArgumentException("Body cannot be null or empty.", nameof(body));
            }
            Body = body;
        }
        public void SetType(NotificationType type)
        {
            Type = type;
        }
        public void SetErrorMessage(string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(errorMessage))
            {
                throw new ArgumentException("Error message cannot be null or empty.", nameof(errorMessage));
            }
            ErrorMessage = errorMessage;
        }
    }
}

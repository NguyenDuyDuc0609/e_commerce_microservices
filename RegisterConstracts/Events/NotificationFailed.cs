using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterConstracts.Events
{
    public record NotificationFailed
    {
        public Guid CorrelationId { get; init; }
        public string? Message { get; init; }
        public Guid? UserId { get; init; }
        public NotificationFailed(Guid correlationId, string? message, Guid? userId)
        {
            CorrelationId = correlationId;
            Message = message;
            UserId = userId;
        }
        public NotificationFailed()
        {
        }
    }
}

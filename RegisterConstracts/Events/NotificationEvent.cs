using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterConstracts.Events
{
    public record NotificationEvent
    {
        public Guid CorrelationId { get; init; }
        public string? Message { get; init; } = default!;
        public NotificationEvent(Guid correlationId, string? message)
        {
            CorrelationId = correlationId;
            Message = message;
        }
        public NotificationEvent()
        {
        }
    }
}

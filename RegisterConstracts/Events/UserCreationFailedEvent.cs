using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterConstracts.Events
{
    public record UserCreationFailedEvent
    {
        public Guid CorrelationId { get; init; }
        public string? Message { get; init; }
        public UserCreationFailedEvent(Guid correlationId, string? message)
        {
            CorrelationId = correlationId;
            Message = message;
        }
        public UserCreationFailedEvent()
        {
        }
    }
}

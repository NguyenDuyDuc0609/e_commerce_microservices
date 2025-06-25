using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterConstracts.Events
{
    public record UserDeletedEvent
    {
        public Guid CorrelationId { get; init; }
        public UserDeletedEvent(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public UserDeletedEvent()
        {
        }
    }
}

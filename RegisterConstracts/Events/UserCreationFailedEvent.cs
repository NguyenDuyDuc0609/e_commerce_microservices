using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterConstracts.Events
{
    public class UserCreationFailedEvent
    {
        public Guid CorrelationId { get; set; }
        public string? Message { get; set; }
    }
}

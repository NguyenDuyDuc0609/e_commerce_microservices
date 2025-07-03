using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterConstracts.Events.ForgotPasswordEvents
{
    public interface ISendTokenResult
    {
        public Guid CorrelationId { get; init; }
        public string? Message { get; init; } 
    }
}

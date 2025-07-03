using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterConstracts.Events.ForgotPasswordEvents
{
    public record CreateTokenFailed
    {
        public Guid CorrelationId { get; init; }
        public string? Message { get; init; } = default!;
        public CreateTokenFailed(Guid correlationId, string? message)
        {
            CorrelationId = correlationId;
            Message = message;
        }
        public CreateTokenFailed()
        {
        }
    }
}

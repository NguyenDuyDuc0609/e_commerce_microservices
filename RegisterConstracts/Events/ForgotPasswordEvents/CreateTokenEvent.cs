using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterConstracts.Events.ForgotPasswordEvents
{
    public record CreateTokenEvent
    {
        public Guid CorrelationId { get; init; }
        public string? Email { get; init; } = default!;
        public string? Token { get; init; } = default!;
        public CreateTokenEvent(Guid correlationId, string? email, string? token)
        {
            CorrelationId = correlationId;
            Email = email;
            Token = token;
        }
        public CreateTokenEvent()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterConstracts.Commands
{
    public record ForgotPasswordCommand
    {
        public Guid CorrelationId { get; init; }
        public string? Email { get; init; }
        public ForgotPasswordCommand(Guid correlationId, string? email)
        {
            CorrelationId = correlationId;
            Email = email;
        }
        public ForgotPasswordCommand()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterConstracts.Commands
{
    public record SendTokenCommand
    {
        public Guid CorrelationId { get; init; }
        public string? Email { get; init; }
        public string? Token { get; set; }
        public SendTokenCommand(Guid correlationId, string? email, string? token)
        {
            CorrelationId = correlationId;
            Email = email;
            Token = token;
        }
        public SendTokenCommand()
        {
        }
    }
}

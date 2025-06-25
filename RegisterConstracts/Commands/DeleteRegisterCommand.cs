using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterConstracts.Commands
{
    public record DeleteRegisterCommand
    {
        public Guid CorrelationId { get; init; }
        public Guid UserId { get; init; }
        public DeleteRegisterCommand(Guid correlationId, Guid userId)
        {
            CorrelationId = correlationId;
            UserId = userId;
        }
        public DeleteRegisterCommand()
        {
        }
    }
}

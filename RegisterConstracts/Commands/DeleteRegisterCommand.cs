using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterConstracts.Commands
{
    public class DeleteRegisterCommand
    {
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; set; }
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

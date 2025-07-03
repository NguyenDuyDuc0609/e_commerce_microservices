using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegisterConstracts.Events.ForgotPasswordEvents
{
    public record EndForgotSagaFailed
    {
        public Guid CorrelationId { get; init; }
    }
}

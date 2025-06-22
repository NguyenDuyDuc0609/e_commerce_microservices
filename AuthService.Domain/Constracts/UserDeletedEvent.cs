using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Constracts
{
    public class UserDeletedEvent
    {
        public Guid CorrelationId { get; set; }
    }
}

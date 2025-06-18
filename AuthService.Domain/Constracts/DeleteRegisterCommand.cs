using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Constracts
{
    public class DeleteRegisterCommand
    {
        public Guid CorrelationId { get; set; }
        public Guid UserId { get; set; }
    }
}

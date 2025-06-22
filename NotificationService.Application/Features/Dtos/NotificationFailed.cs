using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Features.Dtos
{
    public class NotificationFailed
    {
        public Guid CorrelationId { get; set; }
        public string? Message { get; set; }
        public Guid? UserId { get; set; }
    }
}

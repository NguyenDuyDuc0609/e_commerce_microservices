using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Features.Dtos
{
    public class NotificationConsumerResponse
    {
        public Guid CorrelationId { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}

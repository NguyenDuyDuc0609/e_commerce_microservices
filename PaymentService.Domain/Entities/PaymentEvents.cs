using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Domain.Entities
{
    public class PaymentEvents(Guid paymentId, string description, string? additionalData = null)
    {
        public Guid PaymentEventId { get; set; } = Guid.NewGuid();
        public Guid PaymentId { get; set; } = paymentId;
        public DateTime EventDate { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; } = description;
        public string? AdditionalData { get; set; } = additionalData;
        public Payment Payment { get; set; }
    }
}

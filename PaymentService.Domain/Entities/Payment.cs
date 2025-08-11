using PaymentService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Domain.Entities
{
    public class Payment
    {
        public Guid PaymentId { get; set; }
        public decimal Amount { get; private set; }
        public string? Currency { get; set; }
        public PaymentStatus Status { get; private set; }
        public string? ProviderTransactionId { get; private set; }
        public DateTime PaymentDate { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public Payment(decimal amount, string? currency, PaymentStatus status, string? providerTransactionId = null)
        {
            PaymentId = Guid.NewGuid();
            Amount = amount;
            Currency = currency;
            Status = status;
            ProviderTransactionId = providerTransactionId;
            PaymentDate = DateTime.UtcNow;
            CreateAt = DateTime.UtcNow;
            UpdateAt = DateTime.UtcNow;
        }

        public void UpdateStatus(PaymentStatus newStatus)
        {
            Status = newStatus;
            UpdateAt = DateTime.UtcNow;
        }
        public void UpdateProviderTransactionId(string providerTransactionId)
        {
            ProviderTransactionId = providerTransactionId;
            UpdateAt = DateTime.UtcNow;
        }
        public void UpdateAmount(decimal newAmount)
        {
            Amount = newAmount;
            UpdateAt = DateTime.UtcNow;
        }
        public void UpdateCurrency(string? newCurrency)
        {
            Currency = newCurrency;
            UpdateAt = DateTime.UtcNow;
        }
        public List<PaymentEvents> PaymentEvents { get; set; } = new List<PaymentEvents>();

    }
}

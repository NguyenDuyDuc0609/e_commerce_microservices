using OrderService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; private set; } = 0.0m;
        public string Address { get; set; } 
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public OrderStatus OrderStatus { get; set; }
        public Order(Guid userId, string userName, decimal totalAmount, string address, OrderStatus orderStatus)
        {
            OrderId = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            TotalAmount = totalAmount;
            UserId = userId;
            UserName = userName;
            OrderStatus = orderStatus;
            Address = address;
        }
        internal void UpdateOrder(OrderStatus orderStatus)
        {
            OrderStatus = orderStatus;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}

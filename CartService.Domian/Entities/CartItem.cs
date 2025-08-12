using CartService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Domain.Entities
{
    public class CartItem(Guid cartId, Guid productId, string productName, decimal price, int quantity)
    {
        public Guid CartItemId { get; set; } = Guid.NewGuid();
        public Guid CartId { get; set; } = cartId;
        public Guid ProductId { get; set; } = productId;
        public string ProductName { get; set; } = productName;
        public int Quantity { get; private set; } = quantity;
        public decimal Price { get; private set; } = price;
        public StatusItem StatusItem { get; private set; } = StatusItem.InStock;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        public Cart Cart { get; set; } = null!;
        internal void UpdateQuantity(int quantity)
        {
            this.Quantity = quantity;
            UpdatedAt = DateTime.UtcNow;
        }
        public void UpdateStatus(StatusItem status)
        {
            this.StatusItem = status;
            UpdatedAt = DateTime.UtcNow;
        }
        public void UpdatePrice(decimal price)
        {
            this.Price = price;
            UpdatedAt = DateTime.UtcNow;
        }

    }
}

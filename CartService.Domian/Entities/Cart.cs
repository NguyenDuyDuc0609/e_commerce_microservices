using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Domain.Entities
{
    public class Cart
    {
        public Guid CartId { get; set; } = Guid.NewGuid();
        public  Guid UserId { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; private set; } = 0.0m;
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        private Cart() { }
        public Cart(Guid userId)
        {
            UserId = userId;
        }
        public void AddItem(Guid productId, string productName, decimal price, int quantity)
        {
            var exstingItem = Items.FirstOrDefault(i => i.ProductId == productId);
            if (exstingItem != null)
                exstingItem.UpdateQuantity(quantity);
            else
                Items.Add(new CartItem(this.CartId, productId, productName, price, quantity));

        }
        private void UpdateTotalAmount()
        {
            TotalAmount = Items.Sum(i => i.Price * i.Quantity);
            UpdatedAt = DateTime.UtcNow;
        }
        public void RemoveItem(Guid productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                Items.Remove(item);
                UpdateTotalAmount();
            }
        }
    }
}

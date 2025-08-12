namespace OrderService.Domain.Entities
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        public OrderItem(Guid orderId, Guid productId, string productName, decimal price, int quantity)
        {
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }

        internal void UpdateQuantity(int quantity)
        {
            Quantity = quantity;
            UpdatedAt = DateTime.UtcNow;
        }

        internal void UpdatePrice(decimal price)
        {
            Price = price;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
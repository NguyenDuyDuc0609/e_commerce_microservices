using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string Slug { get; private set; } = string.Empty;
        public string Brand { get; private set; } = string.Empty;
        public string ImageUrl { get; private set; } = string.Empty;
        public decimal Price { get; private set; } = 0.0m;
        public bool IsActive { get; private set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        public Category Category { get; set; } = null!;
        public ICollection<SKU> SKUs { get; set; } = new List<SKU>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<ProductDiscount> ProductDiscounts { get; set; } = new List<ProductDiscount>();

        private List<ProductAttribute> _attributes = new();
        public IReadOnlyCollection<ProductAttribute> ProductAttributes => _attributes.AsReadOnly();

        private ProductRatingSummary _ratingSummary = default!;
        public ProductRatingSummary RatingSummary => _ratingSummary;

 

        public Product( Guid categoryId ,string name , string description, string slug, string brand, string imageUrl, decimal price)
        {
            ProductId = Guid.NewGuid();
            CategoryId = categoryId;
            Name = name;
            Description = description;
            Slug = slug;
            Brand = brand;
            ImageUrl = imageUrl;
            Price = price;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
        public void UpdateProduct(string name, string description, string slug, string brand, string imageUrl, decimal price)
        {
            Name = name;
            Description = description;
            Slug = slug;
            Brand = brand;
            ImageUrl = imageUrl;
            Price = price;
            UpdatedAt = DateTime.UtcNow;
        }
        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }
        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }
        public bool SetBrand(string brand)
        {
            if (string.IsNullOrWhiteSpace(brand))
            {
                return false;
            }
            Brand = brand;
            return true;
        }
        public bool SetImageUrl(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                return false;
            }
            ImageUrl = imageUrl;
            return true;
        }
        public bool SetSlug(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return false;
            }
            Slug = slug;
            return true;
        }
        public void AddAttribute(string attributeName, string attributeValue)
        {
            if (string.IsNullOrWhiteSpace(attributeName) || string.IsNullOrWhiteSpace(attributeValue))
            {
                throw new ArgumentException("Attribute name and value cannot be null or empty.");
            }
            _attributes.Add(new ProductAttribute(attributeName, attributeValue));
        }
        public void UpdateAttribute(string attributeName, string attributeValue){
            if (string.IsNullOrWhiteSpace(attributeName) || string.IsNullOrWhiteSpace(attributeValue))
            {
                throw new ArgumentException("Attribute name and value cannot be null or empty.");
            }
            var existingAttribute = _attributes.SingleOrDefault(pa => pa.AttributeName!.Equals(attributeName, StringComparison.OrdinalIgnoreCase));
            if (existingAttribute != null)
            {
                existingAttribute.UpdateAttribute(attributeName, attributeValue);
            }
            else
            {
                _attributes.Add(new ProductAttribute(attributeName, attributeValue));
            }
        }
        public void UpdateRatingSummary(int rating, int? oldRating = 0)
        {
            _ratingSummary.UpdateRating(rating, oldRating);
        }
    }
}

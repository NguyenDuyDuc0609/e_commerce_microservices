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
        public decimal Price { get; private set; }
        public bool IsActive { get; private set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        public Category Category { get; set; } = null!;
        public ICollection<SKU> SKUs { get; set; } = new List<SKU>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<ProductDiscount> ProductDiscounts { get; set; } = new List<ProductDiscount>();
        public ICollection<ProductAttribute> ProductAttributes { get; set; } = new List<ProductAttribute>();
        public ProductRatingSummary? ProductRatingSummaries { get; set; }
        public Product(string name, string description, string slug, string brand, string imageUrl, decimal price)
        {
            Name = name;
            Description = description;
            Slug = slug;
            Brand = brand;
            ImageUrl = imageUrl;
            Price = price;
            IsActive = true;
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
    }
}

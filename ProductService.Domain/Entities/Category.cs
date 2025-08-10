using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    public class Category(string name, string description, bool isActive)
    {
        public Guid CategoryId { get; set; }
        public string Name { get; private set; } = name;
        public string Description { get; private set; } = description;
        public string Slug { get; set; } = string.Empty;
        public bool IsActive { get; private set; } = isActive;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; } = null;
        public ICollection<Product> Products { get; set; } = new List<Product>();

        public void UpdateCategory(string name, string description)
        {
            Name = name;
            Description = description;
            Slug = name.ToLower().Replace(" ", "-");
            UpdatedAt = DateTime.UtcNow;
        }
        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}

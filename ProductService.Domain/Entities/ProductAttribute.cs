using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    [Owned]
    public class ProductAttribute
    {
        public string? AttributeName { get; set; }
        public string? AttributeValue { get; set; }
        private ProductAttribute() { }
        public ProductAttribute(string? attributeName, string? attributeValue)
        {
            AttributeName = attributeName;
            AttributeValue = attributeValue;
        }

        public void UpdateAttribute(string? attributeName, string? attributeValue)
        {
            AttributeName = attributeName;
            AttributeValue = attributeValue;
        }
    }
}

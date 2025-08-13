using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Features.Dtos
{
    public class AddItemDto
    {
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
    }
}

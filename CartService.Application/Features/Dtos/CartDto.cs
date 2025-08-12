using CartService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Features.Dtos
{
    public class CartDto
    {
        public Guid CartId { get; set; } 
        public Guid UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<ItemDto> Items { get; set; } = [];
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Dtos
{
    public class ReviewDto
    {
        public Guid ProductId { get; set; }
        public string? UserName { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }

    }
}

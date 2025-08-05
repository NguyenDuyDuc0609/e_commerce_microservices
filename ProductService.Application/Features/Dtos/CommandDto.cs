using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Dtos
{
    public class CommandDto
    {
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
        public CommandDto(string? message = null, bool isSuccess = true)
        {
            Message = message;
            IsSuccess = isSuccess;
        }
    }
}

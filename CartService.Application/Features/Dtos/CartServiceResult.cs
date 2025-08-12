using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Features.Dtos
{
    public class CartServiceResult(bool isSuccess, string message)
    {
        public bool IsSuccess { get; set; } = isSuccess;
        public string Message { get; set; } = message;
        public object? Data { get; set; } 
    }
}

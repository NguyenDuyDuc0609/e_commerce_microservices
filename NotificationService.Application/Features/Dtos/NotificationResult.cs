using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Features.Dtos
{
    public class NotificationResult
    {
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
        public NotificationResult(string? message, bool isSuccess)
        {
            Message = message;
            IsSuccess = isSuccess;
        }
        public NotificationResult()
        {
        }
    }
}

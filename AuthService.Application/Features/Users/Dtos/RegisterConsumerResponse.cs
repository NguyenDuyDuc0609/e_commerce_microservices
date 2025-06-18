using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Users.Dtos
{
    public class RegisterConsumerResponse
    {
        public Guid CorrelationId { get; set; }
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public Guid? UserId { get; set; }
    }
}

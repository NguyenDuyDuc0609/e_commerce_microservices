using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Application.Dtos
{
    public class MessageResult
    {
        public string? Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}

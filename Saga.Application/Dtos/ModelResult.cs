using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Application.Dtos
{
    public class ModelResult
    {
        public Guid? CorrelationId { get; set; }
        public string? Message { get; set; } = default!;
    }
}

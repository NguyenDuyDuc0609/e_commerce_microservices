using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Application.Interfaces
{
    public interface IExternalHealthChecker
    {
        Task<bool> CheckHealthAsync();
    }
}

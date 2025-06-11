using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Abstractions.Interface
{
    public interface IEntitiyBase<T>
    {
        T Id { get; set; }
    }
}

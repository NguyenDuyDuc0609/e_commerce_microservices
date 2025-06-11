using AuthService.Domain.Abstractions.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Abstractions.Interface
{
    public interface IEntityAuditBase<T> : IEntitiyBase<T>, IUserTracking, IDateTracking, ISoftDelete
    {
    }
}

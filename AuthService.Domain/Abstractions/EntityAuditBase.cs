
using AuthService.Domain.Abstractions.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Abstractions
{
    public abstract class EntityAuditBase<T> : IEntityAuditBase<T> where T : struct
    {
        public abstract T Id { get; set; }
        public abstract string CreatedBy { get; set; }
        public abstract string ModifiedBy { get; set; }
        public abstract DateTimeOffset CreatedDate { get; set; }
        public abstract DateTimeOffset ModifiedDate { get; set; }
        public abstract bool IsDelete { get; set; }
        public abstract DateTimeOffset DeleteDate { get; set; }
    }
}

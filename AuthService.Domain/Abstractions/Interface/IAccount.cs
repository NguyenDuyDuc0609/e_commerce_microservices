using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Abstractions.Interface
{
    public interface IAccount
    {
        public string FullName { get; set; }
        public string? HashedEmail { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}

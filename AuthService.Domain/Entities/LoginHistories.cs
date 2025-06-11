using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class LoginHistories
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string UsernameAttempt { get; set; }
        public string IpAddress { get; set; }
        public string DeviceInfo { get; set; }
        public bool IsSuccessful { get; set; }
        public DateTimeOffset AttemptedAt { get; set; }
        public virtual User? User { get; set; }
        public LoginHistories() { }

    }
}

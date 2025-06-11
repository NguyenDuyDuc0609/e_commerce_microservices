using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class PasswordResetTokens
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public bool IsExpired { get; private set; }
        public bool IsDelete { get; private set; }
        public virtual User? User { get; set; }
        public PasswordResetTokens()
        {
        }
        public PasswordResetTokens(Guid userId, string token, DateTimeOffset expiresAt)
        {
            UserId = userId;
            Token = token;
            CreatedAt = DateTimeOffset.UtcNow;
            ExpiresAt = expiresAt;
            IsExpired = false;
            IsDelete = false;
        }
        public void MarkAsExpired()
        {
            IsExpired = true;
        }
        public void MarkAsDeleted()
        {
            IsDelete = true;
        }
    }
}

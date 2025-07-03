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
        public PasswordResetTokens(Guid userId)
        {
            UserId = userId;
            Token = CreateToken();
            CreatedAt = DateTimeOffset.UtcNow;
            ExpiresAt = DateTimeOffset.UtcNow.AddHours(12);
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
        private static string CreateToken()
        {
            var token = "";
            int randomValue;
            char randomChar;
            Random random = new();
            int tokenLength = random.Next(5, 10);
            for(int i = 0; i < tokenLength; i++)
            {
                randomValue = random.Next(0, 26);
                randomChar = Convert.ToChar(randomValue + 65);
                token += randomChar;
            }
            return token;
        }
    }
}

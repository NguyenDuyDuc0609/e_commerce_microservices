using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
namespace AuthService.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string? HashEmailVerification { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Salt { get; private set; } 
        public string Address { get; private set; }
        public bool IsActive { get; private set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; private set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public User() { }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
        public virtual ICollection<PasswordResetTokens> PasswordResetTokens { get; set; } = new List<PasswordResetTokens>();
        public virtual ICollection<LoginHistories> LoginHistories { get; set; } = new List<LoginHistories>();
        public User(string username, string email, string passwordHash, string phoneNumber, string address)
        {
            UserId = Guid.NewGuid();
            Username = username;
            Email = email;
            SetPasswordHash(passwordHash);
            PhoneNumber = phoneNumber;
            Address = address;
            HashEmailVerification = HashEmail(email);
            IsActive = false;
            CreatedDate = DateTimeOffset.UtcNow;
            ModifiedDate = DateTimeOffset.UtcNow;
            CreatedBy = "User register";
            IsDeleted = false;
        }
        public void Activate()
        {
            IsActive = true;
            HashEmailVerification = null;
            ModifiedDate = DateTimeOffset.UtcNow;
        }
        public void Deactivate()
        {
            IsActive = false;
            ModifiedDate = DateTimeOffset.UtcNow;
        }
        private static string HashPasswordWithSalt(string password, string base64Salt)
        {
            byte[] saltBytes = Convert.FromBase64String(base64Salt);

            byte[] hashBytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32
            );

            return Convert.ToBase64String(hashBytes);
        }
        private void SetPasswordHash(string passwordHash)
        {
            Salt = GenerateSalt();
            PasswordHash = HashPasswordWithSalt(passwordHash, Salt);
        }
        public void SetEmail(string email)
        {
            Email = email;
            ModifiedDate = DateTimeOffset.UtcNow;
        }
        public void MarkAsDeleted()
        {
            IsDeleted = true;
            DeletedDate = DateTimeOffset.UtcNow;
            ModifiedDate = DateTimeOffset.UtcNow;
        }
        public void AssignRole(Guid roleId)
        {
            if (UserRoles.Any(r => r.RoleId == roleId)) return;
            UserRoles.Add(new UserRole(UserId, roleId));
        }

        private static string GenerateSalt(int size = 16)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(size); 
            return Convert.ToBase64String(saltBytes);
        }
        private static string HashEmail(string email)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(email);
            var hashEmail = sha256.ComputeHash(bytes);
            var builder = new StringBuilder();
            foreach (var item in hashEmail)
            {
                builder.Append(item.ToString("x2"));
            }
            return builder.ToString();
        }
        public void SetPhoneNumber(string phoneNumber)
        {
            PhoneNumber = phoneNumber;
            ModifiedDate = DateTimeOffset.UtcNow;
        }
        public void SetAddress(string address)
        {
            Address = address;
            ModifiedDate = DateTimeOffset.UtcNow;
        }
        public bool VerifyPassword(string password)
        {
            string inputPassword = HashPasswordWithSalt(password, Salt);
            return PasswordHash == inputPassword;
        }
    }
}

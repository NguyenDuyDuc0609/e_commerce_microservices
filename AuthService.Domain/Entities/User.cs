using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Address { get; private set; }
        public bool IsActive { get; private set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; private set; }
        public DateTimeOffset? DeletedDate { get; set; }

        public User() { }

        private readonly List<UserRole> _roles = new List<UserRole>();
        public virtual IReadOnlyCollection<UserRole> UserRoles => _roles.AsReadOnly();
        public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
        public virtual ICollection<PasswordResetTokens> PasswordResetTokens { get; set; } = new List<PasswordResetTokens>();
        public virtual ICollection<LoginHistories> LoginHistories { get; set; } = new List<LoginHistories>();
        public User(string username, string email, string passwordHash, string phoneNumber, string address)
        {
            UserId = Guid.NewGuid();
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            PhoneNumber = phoneNumber;
            Address = address;
            IsActive = false;
            CreatedDate = DateTimeOffset.UtcNow;
            ModifiedDate = DateTimeOffset.UtcNow;
            CreatedBy = "User register";
            IsDeleted = false;
        }
        public void Activate()
        {
            IsActive = true;
            ModifiedDate = DateTimeOffset.UtcNow;
        }
        public void Deactivate()
        {
            IsActive = false;
            ModifiedDate = DateTimeOffset.UtcNow;
        }
        public void SetPasswordHash(string passwordHash)
        {
            passwordHash = EncryptMd5(passwordHash);
            ModifiedDate = DateTimeOffset.UtcNow;
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
            if (_roles.Any(r => r.RoleId == roleId)) return;
            _roles.Add(new UserRole(UserId, roleId));
        }
        private static string EncryptMd5(string password)
        {
            MD5 mD5 = MD5.Create();
            byte[] input = Encoding.UTF8.GetBytes(password);
            byte[] output = mD5.ComputeHash(input);
            string passwordHashed = BitConverter.ToString(output).Replace("-", string.Empty);
            return passwordHashed;
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
    }
}

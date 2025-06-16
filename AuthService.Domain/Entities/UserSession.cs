using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class UserSession
    {
        public Guid SessionId { get; set; }
        public Guid UserId { get; set; }
        public string DeviceInfo { get; private set; }
        public string IpAddress { get; private set; }
        public string RefreshToken { get; private set; }
        public DateTimeOffset ExpiryDate { get; set; }
        public DateTimeOffset LoginTime { get; set; }
        public DateTimeOffset? LogoutTime { get; set; }
        public bool IsActive { get; private set; }
        public virtual User? User { get; set; }
        private UserSession() { }
        public UserSession(Guid userId, string deviceInfo, string ipAddress, string refreshToken)
        {
            SessionId = Guid.NewGuid();
            UserId = userId;
            DeviceInfo = deviceInfo;
            IpAddress = ipAddress;
            RefreshToken = refreshToken;
            LoginTime = DateTimeOffset.UtcNow;
            IsActive = true;
            ExpiryDate = DateTimeOffset.UtcNow.AddDays(7);
        }
        public void SetLogoutTime()
        {
            LogoutTime = DateTimeOffset.UtcNow;
            IsActive = false;
        }
        public void UpdateRefreshToken(string newRefreshToken)
        {
            RefreshToken = newRefreshToken;
            LoginTime = DateTimeOffset.UtcNow;
        }
        public void UpdateDeviceInfo(string newDeviceInfo)
        {
            DeviceInfo = newDeviceInfo;
            LoginTime = DateTimeOffset.UtcNow;
        }
        public void UpdateIpAddress(string newIpAddress)
        {
            IpAddress = newIpAddress;
            LoginTime = DateTimeOffset.UtcNow;
        }
    }
}

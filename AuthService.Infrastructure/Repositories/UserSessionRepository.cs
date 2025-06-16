using AuthService.Application.Interfaces;
using AuthService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class UserSessionRepository : IUserSessionRepository
    {
        private readonly AuthDbContext _context;
        public UserSessionRepository(AuthDbContext context)
        {
            _context = context;
        }
        public async Task<bool> DeleteDevice(string deviceInfor, string ipAddress)
        {
            var result = await _context.UserSessions
                .Where(x => x.DeviceInfo == deviceInfor && x.IpAddress == ipAddress)
                .FirstOrDefaultAsync();
            if (result != null)
            {
                result.SetLogoutTime();
                return true;
            }
            return false;
        }

        public async Task<bool> IsAuthenticated(Guid userId, string token, string deviceInfor, string ipAddress)
        {
            var session = await _context.UserSessions
                .Where(x => x.UserId == userId  && x.RefreshToken == token && x.DeviceInfo == deviceInfor && x.IpAddress == ipAddress && x.IsActive)
                .FirstOrDefaultAsync();
            if (session == null)
            {
                return false;
            }
            if(session.ExpiryDate < DateTime.UtcNow)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> Logout(Guid userId, string token, string deviceInfor, string ipAddress)
        {
            var session = await _context.UserSessions
                .Where(x => x.UserId == userId && x.DeviceInfo == deviceInfor && x.IpAddress == ipAddress)
                .FirstOrDefaultAsync();
            if (session != null) {
                session.SetLogoutTime();
                return true;
            }
            return false;
        }

        public Task<bool> LogoutAllDevice(string deviceInfor, string ipAddress)
        {
            throw new NotImplementedException();
        }

        public Task<string> NewLoginDevice(Guid userId, string deviceInfor, string ipAddress)
        {
            throw new NotImplementedException();
        }
    }
}

using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Infrastructure.Persistence;
using AuthService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class UserSessionRepository(AuthDbContext context) : IUserSessionRepository
    {
        private readonly AuthDbContext _context = context;
        public async Task<bool> DeleteDevice(Guid UserId,  string deviceInfor)
        {
            var result = await _context.UserSessions
                .Where(x => x.DeviceInfo == deviceInfor && x.UserId == UserId)
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

        public async Task<Guid> NewLoginDevice(Guid userId, string token, string deviceInfor, string ipAddress)
        {
            var session = new UserSession(userId, deviceInfor, ipAddress, token);
            var result = await _context.UserSessions.AddAsync(session);
            if (result.Entity != null)
            {
                return session.SessionId;
            }
            return Guid.Empty;
        }
    }
}

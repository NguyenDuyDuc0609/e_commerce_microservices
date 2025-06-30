using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface IUserSessionRepository
    {
        Task<bool> IsAuthenticated(Guid userId, string token, string deviceInfor, string ipAddress);
        Task<bool> Logout(Guid userId, string token, string deviceInfor, string ipAddress);
        Task<bool> NewLoginDevice(Guid userId,string token, string deviceInfor, string ipAddress);
        Task<bool> DeleteDevice(string deviceInfor, string ipAddress);
        Task<bool> LogoutAllDevice(string deviceInfor, string ipAddress);
    }
}

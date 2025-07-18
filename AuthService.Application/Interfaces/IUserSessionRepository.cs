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
        Task<Guid> NewLoginDevice(Guid userId,string token, string deviceInfor, string ipAddress);
        Task<bool> DeleteDevice(Guid userId, string deviceInfor);
        Task<bool> LogoutAllDevice(string deviceInfor, string ipAddress);
    }
}

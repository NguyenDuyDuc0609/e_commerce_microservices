using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<bool> AddUserRoleAsync(Guid userId, Guid roleId);
    }
}

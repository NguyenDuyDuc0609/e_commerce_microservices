﻿using AuthService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Guid> GetRole(string roleName);
    }
}

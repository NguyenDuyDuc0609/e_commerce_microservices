﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class UserRole
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public virtual User? User { get; set; }
        public virtual Role? Role { get; set; }
        private UserRole() { }
        public UserRole(Guid userId, Guid roleId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            RoleId = roleId;
        }
    }
}

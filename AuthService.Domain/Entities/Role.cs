using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Entities
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        private Role() { }
        public Role(string roleName, string description)
        {
            RoleId = Guid.NewGuid();
            RoleName = roleName;
            Description = description;
        }

    }
}

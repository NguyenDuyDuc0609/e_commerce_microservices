using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Features.Users.Dtos
{
    public record UpdateCache {
        public Guid SessionId { get; init; }
        public string? RefreshToken { get; init; }
    };
}

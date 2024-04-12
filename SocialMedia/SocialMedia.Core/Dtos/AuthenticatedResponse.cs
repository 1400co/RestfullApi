using System;
using System.Collections.Generic;

namespace TransforSerPu.Core.Dtos
{
    public class AuthenticatedResponse
    {
        public string? AuthToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? ExpiresIn { get; set; } = DateTime.Now.AddHours(24);
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public IEnumerable<RolModuleCombinadoDto>? Permisos { get; set; }

    }
}


using System;

namespace TransforSerPu.Core.Dtos
{
    public class AuthenticatedResponse
    {
        public string? AuthToken { get; set; }
        public string? RefreshToken { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }

    }
}

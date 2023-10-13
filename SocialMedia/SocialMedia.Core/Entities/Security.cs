using SocialMedia.Core.Enumerations;
using System;

namespace SocialMedia.Core.Entities
{
    public class Security : BaseEntity
    {
 
        public string UserName { get; set; }
        public string Password { get; set; }
        
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public RoleType Role { get; set; }


        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}

using SocialMedia.Core.Entities;
using SocialMedia.Core.Enumerations;
using System;

namespace SocialMedia.Core.Dtos
{
    public partial class UserDto : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime BornDate { get; set; }
        public string Phone { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public Subscription Subscription { get; set; }
        public virtual string Roles { get; set; } = string.Empty;
    }
}

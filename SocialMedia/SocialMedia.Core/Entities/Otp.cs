using System;

namespace SocialMedia.Core.Entities
{
    public class Otp : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Password { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}

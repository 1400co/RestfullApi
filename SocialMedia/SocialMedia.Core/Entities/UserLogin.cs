using System;

namespace SocialMedia.Core.Entities
{
    public class UserLogin : BaseEntity
    {
        public string User { get; set; }
        public string Password { get; set; }
        public virtual Guid  UserId { get; set; }
    }
}

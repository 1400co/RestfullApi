using SocialMedia.Core.Entities;
using System;

namespace SocialMedia.Core.Dtos
{
    public class PasswordRecoveryDto : BaseEntity
    {
        public Guid PasswordRecoveryToken { get; set; }
        public DateTime ExpiryDate { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual UserDto User { get; set; }

    }
}

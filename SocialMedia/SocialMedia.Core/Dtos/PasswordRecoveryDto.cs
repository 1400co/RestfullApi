using SocialMedia.Core.Entities;
using System;

namespace SocialMedia.Core.Dtos
{
    public class PasswordRecoveryDto : BaseEntity
    {
        public string UserName { get; set; }
        public Guid PasswordRecoveryToken { get; set; } = Guid.NewGuid();
        public DateTime ExpiryDate { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual UserDto User { get; set; }
    }
}

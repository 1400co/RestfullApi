using System;

namespace SocialMedia.Core.Entities
{
    public class PasswordRecovery : BaseEntity
    {
        public Guid PasswordRecoveryToken { get; set; }
        public DateTime ExpiryDate { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual User User { get; set; }

    }
}

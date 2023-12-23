
using System;

namespace SocialMedia.Core.Entities
{
    public class PasswordRecovery : BaseEntity
    {
        public Guid PasswordRecoveryToken { get; set; } = Guid.NewGuid();
        public DateTime ExpiryDate { get; set; } = DateTime.UtcNow.AddDays(1);
        public virtual Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}

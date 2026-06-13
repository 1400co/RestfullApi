using Template.Core.Entities;
using System;

namespace Template.Core.Dtos
{
    public class PasswordRecoveryDto : BaseEntity
    {
        public string UserName { get; set; } = string.Empty;
        public Guid PasswordRecoveryToken { get; set; } = Guid.NewGuid();
        public DateTime ExpiryDate { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual UserDto? User { get; set; }
    }
}

using System;

namespace Template.Core.Entities
{
    public class Otp : BaseEntity
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public string Password { get; set; } = string.Empty;
        public DateTime? ExpireDate { get; set; }
    }
}

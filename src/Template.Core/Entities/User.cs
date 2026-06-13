
using Template.Core.Enumerations;
using System;
using System.Collections.Generic;

namespace Template.Core.Entities
{
    public partial class User : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime BornDate { get; set; }
        public string Phone { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public Subscription Subscription { get; set; }
        public List<RoleType> Roles { get; set; } = [];

        public virtual ICollection<Comment> Comments { get; set; } = [];
        public virtual ICollection<Post> Post { get; set; } = [];
    }
}


using SocialMedia.Core.Enumerations;
using System;
using System.Collections.Generic;

namespace SocialMedia.Core.Entities
{
    public partial class User : BaseEntity
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Post = new HashSet<Post>();
            Security = new HashSet<Security>();
            UserInRoles = new HashSet<UserInRoles>();
            PasswordRecovery = new HashSet<PasswordRecovery>();
        }

        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BornDate { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public Subscription Subscription { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> Post { get; set; }
        public virtual ICollection<Security> Security { get; set; }
        public virtual ICollection<UserInRoles> UserInRoles { get; set; }
        public virtual ICollection<PasswordRecovery> PasswordRecovery { get; set; }
        
    }
}

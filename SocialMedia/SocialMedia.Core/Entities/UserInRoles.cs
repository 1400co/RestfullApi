using System;
using System.Collections.Generic;

namespace SocialMedia.Core.Entities
{
    public class UserInRoles : BaseEntity
    {
        public virtual Guid UserId { get; set; }
        public virtual User User { get; set; }

        public virtual Guid RoleId { get; set; }
        public virtual Roles Rol { get; set; }

        /*public virtual ICollection<User> Users { get; set; }*/
        /*public virtual ICollection<Roles> Roles { get; set; }*/
        
    }
}

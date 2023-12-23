
using System.Collections.Generic;

namespace SocialMedia.Core.Entities
{
    public  class Roles : BaseEntity
    {
        public string RolName { get; set; }

        public virtual ICollection<RolModule> RolModules { get; set; }
        public virtual ICollection<UserInRoles> UserInRoles { get; set; }
    }
}

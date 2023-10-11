using SocialMedia.Core.Entities;
using System.Collections.Generic;

namespace SocialMedia.Core.Dtos
{
    public class RolesDto : BaseEntity
    {
        public string RolName { get; set; }

        public virtual ICollection<RolModuleDto> RolModules { get; set; }


        public virtual ICollection<UserInRolesDto> UserInRoles { get; set; }
    }
}

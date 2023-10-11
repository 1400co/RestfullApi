﻿using SocialMedia.Core.Dtos;
using System.Collections.Generic;

namespace SocialMedia.Core.Entities
{
    public  class Roles : BaseEntity
    {
        public string RolName { get; set; }

        public virtual ICollection<RolModuleDto> RolModules { get; set; }
        

        public virtual ICollection<UserInRolesDto> UserInRoles { get; set; }
    }
}

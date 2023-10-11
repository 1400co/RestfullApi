﻿using SocialMedia.Core.Dtos;
using System;

namespace SocialMedia.Core.Entities
{
    public class UserInRoles : BaseEntity
    {
        public virtual Guid UserId { get; set; }
        public virtual UserDto User { get; set; }

        public virtual Guid RoleId { get; set; }
        public virtual RolesDto Rol { get; set; }

    }
}

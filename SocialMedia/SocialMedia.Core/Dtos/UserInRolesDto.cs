using SocialMedia.Core.Entities;
using System;

namespace SocialMedia.Core.Dtos
{
    public class UserInRolesDto : BaseEntity
    {
        public virtual Guid UserId { get; set; }
        public virtual UserDto User { get; set; }
        public virtual Guid RoleId { get; set; }
        public virtual RolesDto Rol { get; set; }

    }
}

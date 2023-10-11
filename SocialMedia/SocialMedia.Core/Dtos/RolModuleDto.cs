using SocialMedia.Core.Entities;
using System;

namespace SocialMedia.Core.Dtos
{
    public partial class RolModuleDto : BaseEntity
    {

        public string Module { get; set; }

        public bool Created { get; set; }
        public bool Edited { get; set; }
        public bool Listed { get; set; }
        public bool Deleted { get; set; }
        public bool Printed { get; set; }

        public Guid IdRol { get; set; }
        public virtual RolesDto Rol { get; set; }

    }
}

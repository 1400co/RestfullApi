
using SocialMedia.Core.Enumerations;
using System;

namespace SocialMedia.Core.Entities
{
    public partial class RolModule : BaseEntity
    {
        public bool Created { get; set; }
        public bool Edited { get; set; }
        public bool Listed { get; set; }
        public bool Deleted { get; set; }
        public bool Printed { get; set; }

        public RoleType Role { get; set; }
        public Guid ModuleId { get; set; }
        public virtual Modules Module { get; set; } = null!;
    }
}

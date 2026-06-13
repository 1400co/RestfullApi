using Template.Core.Entities;
using Template.Core.Enumerations;
using System;

namespace Template.Core.Dtos
{
    public partial class RolModuleDto : BaseEntity
    {
        public string Module { get; set; } = string.Empty;
        public bool Created { get; set; }
        public bool Edited { get; set; }
        public bool Listed { get; set; }
        public bool Deleted { get; set; }
        public bool Printed { get; set; }

        public RoleType Role { get; set; }
    }
}

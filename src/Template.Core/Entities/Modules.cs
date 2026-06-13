using System.Collections.Generic;

namespace Template.Core.Entities
{
    public partial class Modules : BaseEntity
    {
        public string ModuleName { get; set; } = string.Empty;
        public virtual ICollection<RolModule> RolModule { get; set; } = [];
    }
}

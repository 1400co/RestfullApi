using System.Collections.Generic;

namespace SocialMedia.Core.Entities
{
    public partial class Modules : BaseEntity
    {
        public string ModuleName { get; set; }
        public virtual ICollection<RolModule> RolModule { get; set; }
    }
}

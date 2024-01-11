
using SocialMedia.Core.Dtos;

namespace TransforSerPu.Core.Dtos
{
    public partial class RolModuleCombinadoDto : BaseDto
    {
        public string Module { get; set; }
        public bool Created { get; set; }
        public bool Edited { get; set; }
        public bool Listed { get; set; }
        public bool Deleted { get; set; }
        public bool Printed { get; set; }
    }
}

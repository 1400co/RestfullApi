
using SocialMedia.Core.Dtos;

namespace TransforSerPu.Core.Dtos
{
    public partial record RolModuleCombinadoDto : BaseDto
    {
        public string Module { get; set; } = string.Empty;
        public bool Created { get; set; }
        public bool Edited { get; set; }
        public bool Listed { get; set; }
        public bool Deleted { get; set; }
        public bool Printed { get; set; }
    }
}

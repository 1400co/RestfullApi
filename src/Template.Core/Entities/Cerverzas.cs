

using Template.Core.Entities;    
namespace Template.Core.Entities
{
    public class Cervezas : BaseEntity
    {
        public string Nombre { get; set; } = string.Empty;
        public decimal GradosAlcohol { get; set; } = 0;
        public decimal Precio { get; set; } = 0;
    }
}
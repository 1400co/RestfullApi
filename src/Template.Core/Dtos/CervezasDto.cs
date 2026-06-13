using System;

namespace Template.Core.Dtos
{
    public record CervezasDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Responsable { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public decimal GradosAlcohol { get; set; }
        public decimal Precio { get; set; }
    }
}

using System;

namespace SocialMedia.Core.Dtos
{
    public abstract record BaseDto
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Responsable { get; set; } = string.Empty;
    }
}

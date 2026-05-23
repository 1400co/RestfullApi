using System;

namespace SocialMedia.Core.Dtos
{
    public record PostDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }
}

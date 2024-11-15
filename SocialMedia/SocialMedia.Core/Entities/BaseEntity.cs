using System;

namespace SocialMedia.Core.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; set; } 
        public string Responsable { get; set; }
    }
}

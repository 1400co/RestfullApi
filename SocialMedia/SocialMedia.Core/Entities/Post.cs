
using System;
using System.Collections.Generic;
using  SocialMedia.Core.Enumerations;

namespace SocialMedia.Core.Entities
{
    public partial class Post : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImageId { get; set; } = string.Empty;
        public string VideoId { get; set; } = string.Empty;
        public PostType PostType { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; } = [];
    }
}

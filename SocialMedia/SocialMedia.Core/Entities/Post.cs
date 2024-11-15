
using System;
using System.Collections.Generic;
using  SocialMedia.Core.Enumerations;

namespace SocialMedia.Core.Entities
{
    public partial class Post : BaseEntity
    {
        public Post()
        {
        }

        public Guid UserId { get; set; }
        public string Description { get; set; }
        public string ImageId { get; set; }
        public string VideoId { get; set; }
        public PostType PostType{ get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
    }
}

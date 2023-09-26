using System;
using System.Collections.Generic;

namespace SocialMedia.Core.Entities
{
    public partial class Comment : BaseEntity
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}

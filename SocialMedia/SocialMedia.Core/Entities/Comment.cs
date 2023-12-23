using System;

namespace SocialMedia.Core.Entities
{
    public partial class Comment : BaseEntity
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }

        //Relations
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}

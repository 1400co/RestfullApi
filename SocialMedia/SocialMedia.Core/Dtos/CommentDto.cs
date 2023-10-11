using SocialMedia.Core.Entities;
using System;

namespace SocialMedia.Core.Dtos
{
    public partial class CommentDto : BaseDto
    {
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }

        //Relations
        public Guid PostId { get; set; }
        public virtual Post Post { get; set; }
        public Guid UserId { get; set; }
        public virtual UserDto User { get; set; }
    }
}

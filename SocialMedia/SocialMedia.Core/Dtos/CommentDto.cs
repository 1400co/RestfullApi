using SocialMedia.Core.Entities;
using System;

namespace SocialMedia.Core.Dtos
{
    public partial record CommentDto : BaseDto
    {
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public Guid PostId { get; set; }
        public virtual Post Post { get; set; } = null!;
        public Guid UserId { get; set; }
        public virtual UserDto User { get; set; } = null!;
    }
}

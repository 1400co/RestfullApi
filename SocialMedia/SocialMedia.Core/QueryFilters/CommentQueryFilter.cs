using System;

namespace SocialMedia.Core.QueryFilters
{
    public class CommentQueryFilter : BaseQueryFilter
    {
        public Guid? UserId { get; set; }
        public string? Description { get; set; }
    }
}

using System;

namespace SocialMedia.Core.QueryFilters
{
    public class CommentQueryFilter
    {
        public Guid? UserId { get; set; }
        public string? Description { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}

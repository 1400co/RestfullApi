using System;

namespace Template.Core.QueryFilters
{
    public class CommentQueryFilter : BaseQueryFilter
    {
        public Guid? UserId { get; set; }
        public string? Description { get; set; }
    }
}

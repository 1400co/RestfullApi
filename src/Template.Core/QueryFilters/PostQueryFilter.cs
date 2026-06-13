using System;

namespace Template.Core.QueryFilters
{
    public class PostQueryFilter : BaseQueryFilter
    {
        public Guid? UserId { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
    }
}

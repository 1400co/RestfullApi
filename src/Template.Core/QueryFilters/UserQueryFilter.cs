using System;

namespace Template.Core.QueryFilters
{
    public class UserQueryFilter : BaseQueryFilter
    {
        public Guid? UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? BornDate { get; set; }
        public string? Phone { get; set; }
        public string? IsActive { get; set; }
        public string? Subscription { get; set; }
      
    }
}

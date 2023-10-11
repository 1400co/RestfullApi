using System;

namespace SocialMedia.Core.QueryFilters
{
    public class RolModuleQueryFilter
    {
        public Guid? RoleId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}

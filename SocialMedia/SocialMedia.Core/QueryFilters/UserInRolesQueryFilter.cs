using System;

namespace SocialMedia.Core.QueryFilters
{
    public class UserInRolesQueryFilter : BaseQueryFilter
    {
        public Guid? UserId { get; set; }
        public Guid? RoleId { get; set; }
       
    }
}

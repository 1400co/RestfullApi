using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IUserInRolesService
    {
        Task DeleteUserInRole(Guid id);
        Task<UserInRoles> GetUserInRole(Guid id);
        PagedList<UserInRoles> GetUserInRoles(UserInRolesQueryFilter filters);
        Task InsertUserInRole(UserInRoles userInRole);
        Task<bool> UpdateUserInRole(UserInRoles userInRole);
    }
}
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IRolesService
    {
        Task DeleteRole(Guid id);
        Task<Roles> GetRole(Guid id);
        PagedList<Roles> GetRoles(RolesQueryFilter filters);
        Task InsertRole(Roles role);
        Task<bool> UpdateRole(Roles role);
    }
}
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IRolesService
    {
        Task DeleteRole(Guid id);
        Task<Roles> GetRole(Guid id);
        Task<PagedList<Roles>> GetRoles(RolesQueryFilter filters);
        Task InsertRole(Roles role);
        Task<bool> UpdateRole(Roles role);
        IEnumerable<Roles> GetAll();
    }
}
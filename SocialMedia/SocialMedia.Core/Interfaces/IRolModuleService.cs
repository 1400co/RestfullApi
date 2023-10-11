using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IRolModuleService
    {
        Task DeleteRolModule(Guid id);
        Task<RolModule> GetRolModule(Guid id);
        PagedList<RolModule> GetRolModules(RolModuleQueryFilter filters);
        Task InsertRolModule(RolModule rolModule);
        Task<bool> UpdateRolModule(RolModule rolModule);
    }
}
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransforSerPu.Core.Dtos;

namespace SocialMedia.Core.Interfaces
{
    public interface IRolModuleService
    {
        Task DeleteRolModule(Guid id);
        Task<RolModule> GetRolModule(Guid id);
        Task<PagedList<RolModule>> GetRolModules(RolModuleQueryFilter filters);
        Task InsertRolModule(RolModule rolModule);
        Task<bool> UpdateRolModule(RolModule rolModule);
        IEnumerable<RolModuleCombinadoDto> ObtenerModulosUsuario(Guid userId);
    }
}
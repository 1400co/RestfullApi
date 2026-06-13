using Template.Core.CustomEntities;
using Template.Core.Entities;
using Template.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template.Core.Dtos;

namespace Template.Core.Interfaces
{
    public interface IRolModuleService
    {
        Task DeleteRolModule(Guid id);
        Task<RolModule?> GetRolModule(Guid id);
        Task<PagedList<RolModule>> GetRolModules(RolModuleQueryFilter filters);
        Task InsertRolModule(RolModule rolModule);
        Task<bool> UpdateRolModule(RolModule rolModule);
        Task<IEnumerable<RolModuleCombinadoDto>> ObtenerModulosUsuario(Guid userId);
    }
}
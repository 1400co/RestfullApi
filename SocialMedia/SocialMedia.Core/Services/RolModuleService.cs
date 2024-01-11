using Microsoft.Extensions.Options;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransforSerPu.Core.Dtos;

namespace SocialMedia.Core.Services
{
    public class RolModuleService : IRolModuleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public RolModuleService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = paginationOptions.Value;
        }

        public async Task InsertRolModule(RolModule rolModule)
        {
            var role = await _unitOfWork.RolesRepository.GetById(rolModule.IdRol);
            if (role == null)
                throw new BusinessException("Role doesn't exist");

            // Additional business validations if necessary

            await _unitOfWork.RolModuleRepository.Insert(rolModule);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateRolModule(RolModule rolModule)
        {
            var existingRolModule = await _unitOfWork.RolModuleRepository.GetById(rolModule.Id);
            if (existingRolModule == null)
                throw new BusinessException("RolModule doesn't exist");

            // Complete properties mapping
            rolModule.CopyPropertiesTo(existingRolModule);

            await _unitOfWork.RolModuleRepository.Update(existingRolModule);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<RolModule> GetRolModule(Guid id)
        {
            return await _unitOfWork.RolModuleRepository.GetById(id);
        }

        public async Task<PagedList<RolModule>> GetRolModules(RolModuleQueryFilter filters)
        {
            var rolModules = _unitOfWork.RolModuleRepository.Get();

            // Using provided filters directly
            filters.PageNumber = filters.PageNumber;
            filters.PageSize = filters.PageSize;

            // Additional filtering logic if necessary
            // (Add properties to RolModuleQueryFilter as needed)
            if (filters.RoleId != null)
            {
                rolModules = rolModules.Where(x => x.IdRol == filters.RoleId);
            }

            var pagedRolModules = await PagedList<RolModule>.CreateAsync(rolModules, filters.PageNumber, filters.PageSize);

            return pagedRolModules;
        }


        public async Task DeleteRolModule(Guid id)
        {
            await _unitOfWork.RolModuleRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public IEnumerable<RolModuleCombinadoDto> ObtenerModulosUsuario(Guid userId)
        {
            var listaRoles = this._unitOfWork.UserInRolesRepository
                .Get().Where(x => x.UserId == userId).Select(x => x.RoleId).ToList();

            //Si contiene el rol administrador, responde full permisos a todos los modulos.
            if (listaRoles.Contains(Guid.Parse("3A9A7CE2-9A5C-4AFF-A47A-C5FDFCD955AE")))
            {
                return this._unitOfWork.ModuleRepository.Get()
                .Select(x => new RolModuleCombinadoDto()
                {
                    Module = x.ModuleName,
                    Created = true,
                    Deleted = true,
                    Edited = true,
                    Listed = true,
                    Printed = true
                }).ToList();
            }

            var rolesModules = this._unitOfWork.RolModuleRepository.Get(x => x.Module, y => y.Rol)
                .Where(x => listaRoles.Contains(x.IdRol)).ToList();

            var combinedPermissions = CombineRolesPermissions(rolesModules)
                .Select(x => new RolModuleCombinadoDto()
                {
                    Module = x.Module.ModuleName,
                    Created = x.Created,
                    Deleted = x.Deleted,
                    Edited = x.Edited,
                    Listed = x.Listed,
                    Printed = x.Printed,
                }).ToList();

            return combinedPermissions;
        }

        private RolModule MergePermissions(RolModule perm1, RolModule perm2)
        {
            return new RolModule
            {
                Module = perm1.Module,
                Created = perm1.Created || perm2.Created,
                Edited = perm1.Edited || perm2.Edited,
                Listed = perm1.Listed || perm2.Listed,
                Deleted = perm1.Deleted || perm2.Deleted,
                Printed = perm1.Printed || perm2.Printed
            };
        }

        public IEnumerable<RolModule> CombineRolesPermissions(List<RolModule> rolesModules)
        {
            return rolesModules
                .GroupBy(rm => rm.Module) // Agrupar por módulo
                .Select(group => group.Aggregate((merged, next) => MergePermissions(merged, next))) // Combina los permisos
                .ToList();
        }
    }

}

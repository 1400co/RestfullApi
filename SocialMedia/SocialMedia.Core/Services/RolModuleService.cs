using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Enumerations;
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
    public class RolModuleService : GenericService<RolModule>, IRolModuleService
    {
        public RolModuleService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions)
            : base(unitOfWork, paginationOptions)
        {
        }

        public async Task InsertRolModule(RolModule rolModule)
        {
            await base.Insert(rolModule).ConfigureAwait(false);
        }

        public async Task<bool> UpdateRolModule(RolModule rolModule)
        {
            return await base.Update(rolModule).ConfigureAwait(false);
        }

        public async Task<RolModule?> GetRolModule(Guid id)
        {
            return await base.Get(id).ConfigureAwait(false);
        }

        public async Task<PagedList<RolModule>> GetRolModules(RolModuleQueryFilter filters)
        {
            var rolModules = _unitOfWork.RolModuleRepository.Get();

            var pagedRolModules = await PagedList<RolModule>.CreateAsync(rolModules, filters.PageNumber, filters.PageSize).ConfigureAwait(false);

            return pagedRolModules;
        }

        public async Task DeleteRolModule(Guid id)
        {
            await base.Delete(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<RolModuleCombinadoDto>> ObtenerModulosUsuario(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetById(userId).ConfigureAwait(false);
            if (user == null)
                return Enumerable.Empty<RolModuleCombinadoDto>();

            var userRoles = user.Roles;

            if (userRoles.Contains(RoleType.Administrator))
            {
                return await _unitOfWork.ModuleRepository.Get()
                    .Select(x => new RolModuleCombinadoDto()
                    {
                        Module = x.ModuleName,
                        Created = true,
                        Deleted = true,
                        Edited = true,
                        Listed = true,
                        Printed = true
                    }).ToListAsync().ConfigureAwait(false);
            }

            var rolesModules = await _unitOfWork.RolModuleRepository.Get(null, x => x.Module)
                .Where(x => userRoles.Contains(x.Role)).ToListAsync().ConfigureAwait(false);

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
                .GroupBy(rm => rm.Module)
                .Select(group => group.Aggregate((merged, next) => MergePermissions(merged, next)))
                .ToList();
        }
    }
}

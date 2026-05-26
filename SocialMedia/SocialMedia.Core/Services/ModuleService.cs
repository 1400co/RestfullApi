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

namespace SocialMedia.Core.Services
{
    public class ModuleService : GenericService<Modules>, IModuleService
    {
        public ModuleService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions)
            : base(unitOfWork, paginationOptions)
        {
        }

        public IEnumerable<Modules> GetAll()
        {
            return base.GetAll();
        }

        public new async Task Insert(Modules input)
        {
            if (string.IsNullOrEmpty(input.ModuleName))
                throw new BusinessException("Role name is required");

            await _unitOfWork.ModuleRepository.Insert(input).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public new async Task<bool> Update(Modules role)
        {
            var existingRole = await _unitOfWork.ModuleRepository.GetById(role.Id).ConfigureAwait(false);
            if (existingRole == null)
                throw new BusinessException("Role doesn't exist");

            existingRole.ModuleName = role.ModuleName;

            await _unitOfWork.ModuleRepository.Update(existingRole).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public new async Task<Modules?> Get(Guid id)
        {
            return await base.Get(id).ConfigureAwait(false);
        }

        public PagedList<Modules> Get(ModulesQueryFilter filters)
        {
            var Module = _unitOfWork.ModuleRepository.Get();

            if (!string.IsNullOrEmpty(filters.Filter))
            {
                Module = Module.Where(x => x.ModuleName.ToLower()
                .Contains(filters.Filter.ToLower()));
            }

            var pagedModule = PagedList<Modules>.Create(Module, filters.PageNumber, filters.PageSize);

            return pagedModule;
        }

        public new async Task Delete(Guid id)
        {
            await base.Delete(id).ConfigureAwait(false);
        }
    }
}

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
    public class ModuleService(IUnitOfWork unitOfWork) : IModuleService
    {

        public IEnumerable<Modules> GetAll()
        {
            var barrios = unitOfWork.ModuleRepository.Get();
            return barrios;
        }


        public async Task Insert(Modules input)
        {
            if (string.IsNullOrEmpty(input.ModuleName))
                throw new BusinessException("Role name is required");

            // Additional business validations if necessary

            await unitOfWork.ModuleRepository.Insert(input).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> Update(Modules role)
        {
            var existingRole = await unitOfWork.ModuleRepository.GetById(role.Id).ConfigureAwait(false);
            if (existingRole == null)
                throw new BusinessException("Role doesn't exist");

            existingRole.ModuleName = role.ModuleName;

            await unitOfWork.ModuleRepository.Update(existingRole).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public async Task<Modules?> Get(Guid id)
        {
            return await unitOfWork.ModuleRepository.GetById(id).ConfigureAwait(false);
        }

        public PagedList<Modules> Get(ModulesQueryFilter filters)
        {
            var Module = unitOfWork.ModuleRepository.Get();

            // Set default pagination values or use provided filters
            filters.PageNumber = filters.PageNumber;
            filters.PageSize = filters.PageSize;

            // Additional filtering logic if necessary
            if (!string.IsNullOrEmpty(filters.Filter))
            {
                Module = Module.Where(x => x.ModuleName.ToLower()
                .Contains(filters.Filter.ToLower()));
            }

            var pagedModule = PagedList<Modules>.Create(Module, filters.PageNumber, filters.PageSize);

            return pagedModule;
        }

        public async Task Delete(Guid id)
        {
            await unitOfWork.ModuleRepository.Delete(id).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
    }

}

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
    public class ModuleService : IModuleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public ModuleService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = paginationOptions.Value;
        }

        public IEnumerable<Modules> GetAll()
        {
            var barrios = _unitOfWork.ModuleRepository.Get();
            return barrios;
        }


        public async Task Insert(Modules input)
        {
            if (string.IsNullOrEmpty(input.ModuleName))
                throw new BusinessException("Role name is required");

            // Additional business validations if necessary

            await _unitOfWork.ModuleRepository.Insert(input);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> Update(Modules role)
        {
            var existingRole = await _unitOfWork.ModuleRepository.GetById(role.Id);
            if (existingRole == null)
                throw new BusinessException("Role doesn't exist");

            existingRole.ModuleName = role.ModuleName;

            await _unitOfWork.ModuleRepository.Update(existingRole);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Modules> Get(Guid id)
        {
            return await _unitOfWork.ModuleRepository.GetById(id);
        }

        public PagedList<Modules> Get(ModulesQueryFilter filters)
        {
            var Module = _unitOfWork.ModuleRepository.Get();

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
            await _unitOfWork.ModuleRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}

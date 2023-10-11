using Microsoft.Extensions.Options;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            existingRolModule.Module = rolModule.Module;
            existingRolModule.Created = rolModule.Created;
            existingRolModule.Edited = rolModule.Edited;
            existingRolModule.Listed = rolModule.Listed;
            existingRolModule.Deleted = rolModule.Deleted;
            existingRolModule.Printed = rolModule.Printed;
            existingRolModule.IdRol = rolModule.IdRol;

            await _unitOfWork.RolModuleRepository.Update(existingRolModule);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<RolModule> GetRolModule(Guid id)
        {
            return await _unitOfWork.RolModuleRepository.GetById(id);
        }

        public PagedList<RolModule> GetRolModules(RolModuleQueryFilter filters)
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

            var pagedRolModules = PagedList<RolModule>.Create(rolModules, filters.PageNumber, filters.PageSize);

            return pagedRolModules;
        }


        public async Task DeleteRolModule(Guid id)
        {
            await _unitOfWork.RolModuleRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}

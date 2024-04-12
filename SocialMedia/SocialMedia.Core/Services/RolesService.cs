using Microsoft.Extensions.Options;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class RolesService : IRolesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public RolesService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = paginationOptions.Value;
        }

        public IEnumerable<Roles> GetAll()
        {
            var barrios = _unitOfWork.RolesRepository.Get();
            return barrios;
        }

        public async Task InsertRole(Roles role)
        {
            if (string.IsNullOrEmpty(role.RolName))
                throw new BusinessException("Role name is required");

            // Additional business validations if necessary

            await _unitOfWork.RolesRepository.Insert(role);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateRole(Roles role)
        {
            var existingRole = await _unitOfWork.RolesRepository.GetById(role.Id);
            if (existingRole == null)
                throw new BusinessException("Role doesn't exist");

            role.CopyPropertiesTo(existingRole);

            await _unitOfWork.RolesRepository.Update(existingRole);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Roles> GetRole(Guid id)
        {
            return await _unitOfWork.RolesRepository.GetById(id);
        }

        public async Task<PagedList<Roles>> GetRoles(RolesQueryFilter filters)
        {
            var roles = _unitOfWork.RolesRepository.Get();

            // Set default pagination values or use provided filters
            filters.PageNumber = filters.PageNumber;
            filters.PageSize = filters.PageSize;

            // Additional filtering logic if necessary
            if (!string.IsNullOrEmpty(filters.Filter))
            {
                roles = roles.Where(x => x.RolName.ToLower().Contains(filters.Filter.ToLower()));
            }

            var pagedRoles = await PagedList<Roles>.CreateAsync(roles, filters.PageNumber, filters.PageSize);

            return pagedRoles;
        }

        public async Task DeleteRole(Guid id)
        {
            await _unitOfWork.RolesRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}

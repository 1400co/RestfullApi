using Microsoft.EntityFrameworkCore;
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
    public class UserInRolesService : IUserInRolesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public UserInRolesService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = paginationOptions.Value;
        }

        public IEnumerable<UserInRoles> GetAll(Guid userId)
        {
            var barrios = _unitOfWork.UserInRolesRepository.Get(x => x.User, y => y.Roles)
                .Where(x => x.UserId == userId);
            return barrios;
        }

        public async Task InsertUserInRole(UserInRoles userInRole)
        {
            if (userInRole.UserId == Guid.Empty || userInRole.RoleId == Guid.Empty)
                throw new BusinessException("UserId and RoleId are required");

            // Additional business validations if necessary

            await _unitOfWork.UserInRolesRepository.Insert(userInRole);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateUserInRole(UserInRoles userInRole)
        {
            var existingUserInRole = await _unitOfWork.UserInRolesRepository.GetById(userInRole.Id);
            if (existingUserInRole == null)
                throw new BusinessException("UserInRole doesn't exist");

            userInRole.CopyPropertiesTo(existingUserInRole);

            await _unitOfWork.UserInRolesRepository.Update(existingUserInRole);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<UserInRoles> GetUserInRole(Guid id)
        {
            return await _unitOfWork.UserInRolesRepository.GetById(id);
        }

        public async Task<List<UserInRoles>> GetUsersRoles(Guid id)
        {
            return await _unitOfWork.UserInRolesRepository.Get(x => x.Roles, y => y.User)
                .Where(x => x.UserId == id).ToListAsync();
        }

        public async Task<PagedList<UserInRoles>> GetUserInRoles(UserInRolesQueryFilter filters)
        {
            var userInRoles = _unitOfWork.UserInRolesRepository.Get();

            filters.PageNumber = filters.PageNumber;
            filters.PageSize = filters.PageSize;

            var pagedUserInRoles = await PagedList<UserInRoles>.CreateAsync(userInRoles, filters.PageNumber, filters.PageSize);

            return pagedUserInRoles;
        }

        public async Task DeleteUserInRole(Guid id)
        {
            await _unitOfWork.UserInRolesRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}

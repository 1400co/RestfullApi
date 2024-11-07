using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Dtos;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task InsertUser(User user)
        {
            if (string.IsNullOrEmpty(user.Email))
                throw new BusinessException("Email is required");

            await _unitOfWork.UserRepository.Insert(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateUser(User user)
        {
            var existingUser = await _unitOfWork.UserRepository.GetById(user.Id);
            if (existingUser == null)
                throw new BusinessException("User doesn't exist");

            user.CopyPropertiesTo(existingUser);

            await _unitOfWork.UserRepository.Update(existingUser);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<User> GetUser(Guid id)
        {
            return await _unitOfWork.UserRepository.GetById(id);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _unitOfWork.UserRepository.Get().FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<PagedList<User>> GetUsers(BaseQueryFilter filters)
        {
            var users = _unitOfWork.UserRepository.Get(null, x=> x.UserInRoles);

            filters.PageNumber = filters.PageNumber;
            filters.PageSize = filters.PageSize;

            if (!string.IsNullOrEmpty(filters.Filter))
            {
                users = users.Where(x => x.Email.ToLower().Contains(filters.Filter.ToLower()));
                users = users.Where(x => x.FullName.ToLower().Contains(filters.Filter.ToLower()));
            }

            var pagedUsers = await PagedList<User>.CreateAsync(users, filters.PageNumber, filters.PageSize);

            return pagedUsers;
        }

        public async Task DeleteUser(Guid id)
        {
            await _unitOfWork.UserRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}

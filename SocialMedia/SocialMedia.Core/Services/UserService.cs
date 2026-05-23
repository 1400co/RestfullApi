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
    public class UserService(IUnitOfWork unitOfWork) : IUserService
    {

        public async Task InsertUser(User user)
        {
            if (string.IsNullOrEmpty(user.Email))
                throw new BusinessException("Email is required");

            await unitOfWork.UserRepository.Insert(user).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> UpdateUser(User user)
        {
            var existingUser = await unitOfWork.UserRepository.GetById(user.Id).ConfigureAwait(false);
            if (existingUser == null)
                throw new BusinessException("User doesn't exist");

            user.CopyPropertiesTo(existingUser);

            await unitOfWork.UserRepository.Update(existingUser).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public async Task<User?> GetUser(Guid id)
        {
            return await unitOfWork.UserRepository.GetById(id).ConfigureAwait(false);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await unitOfWork.UserRepository.Get().FirstOrDefaultAsync(x => x.Email == email).ConfigureAwait(false);
        }

        public async Task<PagedList<User>> GetUsers(BaseQueryFilter filters)
        {
            var users = unitOfWork.UserRepository.Get();

            filters.PageNumber = filters.PageNumber;
            filters.PageSize = filters.PageSize;

            if (!string.IsNullOrEmpty(filters.Filter))
            {
                users = users.Where(x => x.Email.ToLower().Contains(filters.Filter.ToLower()));
                users = users.Where(x => x.FullName.ToLower().Contains(filters.Filter.ToLower()));
            }

            var pagedUsers = await PagedList<User>.CreateAsync(users, filters.PageNumber, filters.PageSize).ConfigureAwait(false);

            return pagedUsers;
        }

        public async Task DeleteUser(Guid id)
        {
            await unitOfWork.UserRepository.Delete(id).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
    }

}

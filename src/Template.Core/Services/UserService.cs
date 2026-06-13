using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Template.Core.CustomEntities;
using Template.Core.Dtos;
using Template.Core.Entities;
using Template.Core.Exceptions;
using Template.Core.Interfaces;
using Template.Core.QueryFilters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Template.Core.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        public UserService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions)
            : base(unitOfWork, paginationOptions)
        {
        }

        public async Task InsertUser(User user)
        {
            if (string.IsNullOrEmpty(user.Email))
                throw new BusinessException("Email is required");

            await _unitOfWork.UserRepository.Insert(user).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> UpdateUser(User user)
        {
            return await base.Update(user).ConfigureAwait(false);
        }

        public async Task<User?> GetUser(Guid id)
        {
            return await base.Get(id).ConfigureAwait(false);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _unitOfWork.UserRepository.Get().FirstOrDefaultAsync(x => x.Email == email).ConfigureAwait(false);
        }

        public async Task<PagedList<User>> GetUsers(BaseQueryFilter filters)
        {
            var users = _unitOfWork.UserRepository.Get();

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
            await base.Delete(id).ConfigureAwait(false);
        }
    }
}

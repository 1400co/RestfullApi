using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IUserService
    {
        Task DeleteUser(Guid id);
        Task<User> GetUser(Guid id);
        PagedList<User> GetUsers(UserQueryFilter filters);
        Task InsertUser(User user);
        Task<bool> UpdateUser(User user);
    }
}
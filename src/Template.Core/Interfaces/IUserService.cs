using Template.Core.CustomEntities;
using Template.Core.Entities;
using Template.Core.QueryFilters;
using System;
using System.Threading.Tasks;

namespace Template.Core.Interfaces
{
    public interface IUserService
    {
        Task DeleteUser(Guid id);
        Task<User?> GetUser(Guid id);
        Task<PagedList<User>> GetUsers(BaseQueryFilter filters);
        Task InsertUser(User user);
        Task<bool> UpdateUser(User user);
        Task<User?> GetUserByEmail(string email);
    }
}
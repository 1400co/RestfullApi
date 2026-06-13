using Template.Core.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDto> GetUser(int id);
        Task<IEnumerable<UserDto>> GetUsers();
    }
}
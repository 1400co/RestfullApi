using SocialMedia.Core.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDto> GetUser(int id);
        Task<IEnumerable<UserDto>> GetUsers();
    }
}
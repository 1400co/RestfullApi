using SocialMedia.Core.Entities;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface ISecurityService
    {
        Task<Security> GetLoginByCredentials(UserLoginDto userLogin);
        Task RegisterUser(Security security);
        Task UpdateRefreshToken(string userName, string refreshToken);
    }
}
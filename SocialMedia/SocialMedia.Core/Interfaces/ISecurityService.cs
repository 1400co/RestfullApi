using SocialMedia.Core.Entities;
using System;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface ISecurityService
    {
        Task<Security> GetCredentialsByUserName(string userLogin);
        Task<Security> GetLoginByCredentials(UserLoginDto userLogin);
        Task RegisterUser(Security security);
        Task UpdateRefreshToken(string userName, string refreshToken);
        Task UpdateRecoveryPassword(Guid userId, string password);
        Task UpdateCredentials(Security data);
        Task<Security> GetSecurityUser(Guid idUsuario);
    }
}
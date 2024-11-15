using SocialMedia.Core.Entities;
using System;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface ISecurityService
    {
        Task<Otp> GetOneTimePassword(Guid userId);
        Task<User> GetUserByEmail(string email);
        Task<bool> ValidateCredentials(string userLogin, string oneTimePassword);
    }
}
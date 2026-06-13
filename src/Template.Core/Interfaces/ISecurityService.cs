using Template.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Template.Core.Interfaces
{
    public interface ISecurityService
    {
        Task<Otp> GetOneTimePassword(Guid userId);
        Task<User?> GetUserByEmail(string email);
        Task<bool> ValidateCredentials(string userLogin, string oneTimePassword);
    }
}
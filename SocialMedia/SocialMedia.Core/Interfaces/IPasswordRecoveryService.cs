using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IPasswordRecoveryService
    {
        Task DeleteRecovery(Guid token);
        Task<PasswordRecovery> GetRecovery(Guid token);
        Task InsertRecovery(PasswordRecovery recovery);
        Task<bool> UpdateRecovery(PasswordRecovery recovery);
        Task<PagedList<PasswordRecovery>> GetRecovery(GeneralQueryFilter filters);
    }
}
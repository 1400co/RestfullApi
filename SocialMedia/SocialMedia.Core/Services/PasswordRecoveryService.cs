using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PasswordRecoveryService : IPasswordRecoveryService
    {
        private readonly IUnitOfWork _unitOfWork;
        // Consider adding more dependencies if necessary, like sending emails

        public PasswordRecoveryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PasswordRecovery> InsertRecovery(PasswordRecovery recovery)
        {
            var user = await _unitOfWork.UserRepository.GetById(recovery.UserId);
            if (user == null)
                throw new BusinessException("User doesn't exist");

            await _unitOfWork.PasswordRecoveryRepository.Insert(recovery);
            await _unitOfWork.SaveChangesAsync();

            return recovery;
        }

        public async Task<bool> UpdateRecovery(PasswordRecovery recovery)
        {
            var existingRecovery = await _unitOfWork.PasswordRecoveryRepository.GetById(recovery.PasswordRecoveryToken);
            if (existingRecovery == null)
                throw new BusinessException("Recovery Token doesn't exist");

            recovery.CopyPropertiesTo(existingRecovery);

            await _unitOfWork.PasswordRecoveryRepository.Update(existingRecovery);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<PasswordRecovery> GetRecovery(Guid token)
        {
            return await _unitOfWork.PasswordRecoveryRepository.GetById(token);
        }

        public async Task DeleteRecovery(Guid token)
        {
            await _unitOfWork.PasswordRecoveryRepository.Delete(token);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<PagedList<PasswordRecovery>> GetRecovery(GeneralQueryFilter filters)
        {
            var pRecovery = _unitOfWork.PasswordRecoveryRepository.Get();

            // Set default pagination values or use provided filters
            filters.PageNumber = filters.PageNumber;
            filters.PageSize = filters.PageSize;

            // Additional filtering logic if necessary
            if (!string.IsNullOrEmpty(filters.Filter))
            {
                pRecovery = pRecovery.Where(x => x.User.FullName.ToLower().Contains(filters.Filter.ToLower()));
            }

            var pagedRecovery = await PagedList<PasswordRecovery>.CreateAsync(pRecovery, filters.PageNumber, filters.PageSize);

            return pagedRecovery;
        }
    }

}

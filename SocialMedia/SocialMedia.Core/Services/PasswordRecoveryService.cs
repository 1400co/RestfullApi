using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using System;
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

        public async Task InsertRecovery(PasswordRecovery recovery)
        {
            var user = await _unitOfWork.UserRepository.GetById(recovery.UserId);
            if (user == null)
                throw new BusinessException("User doesn't exist");

            // Add additional business validations if necessary

            await _unitOfWork.PasswordRecoveryRepository.Insert(recovery);
            await _unitOfWork.SaveChangesAsync();
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
    }

}

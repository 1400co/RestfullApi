using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SecurityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Security> GetLoginByCredentials(UserLoginDto userLogin)
        {
            return await _unitOfWork.SecurityRepository.GetLoginByCredentials(userLogin);
        }

        public async Task RegisterUser(Security security)
        {
            await _unitOfWork.SecurityRepository.Add(security);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateRefreshToken(string userName, string refreshToken)
        {
            var sec = _unitOfWork.SecurityRepository
                .Get().Where(x => x.UserName == userName).FirstOrDefault();

            sec.RefreshToken = refreshToken;
            sec.RefreshTokenExpiryTime = System.DateTime.Now.AddDays(7);

            await _unitOfWork.SecurityRepository.Update(sec);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
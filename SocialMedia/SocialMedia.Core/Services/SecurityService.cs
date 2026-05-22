using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System;
using System.Security.Cryptography;
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

        public async Task<Otp> GetOneTimePassword(Guid userId)
        {
            // Buscar un OTP existente para el usuario
            var otp = await _unitOfWork.GetRepository<Otp>()
                .Get(x => x.UserId == userId && x.ExpireDate > DateTime.UtcNow)
                .SingleOrDefaultAsync();

            // Si existe un OTP y no ha expirado, devolverlo
            if (otp != null)
            {
                return otp;
            }

            // Si no existe un OTP o ha expirado, generar uno nuevo
            otp = new Otp
            {
                UserId = userId,
                Password = GenerateOtpCode(), // Método para generar el código OTP
                ExpireDate = DateTime.UtcNow.AddMinutes(5), // Expira en 5 minutos
                CreatedAt = DateTime.UtcNow,
                Responsable = "System" // Puedes ajustar esto según tus necesidades
            };

            // Guardar el nuevo OTP en la base de datos
            await _unitOfWork.GetRepository<Otp>().AddAsync(otp);
            await _unitOfWork.SaveChangesAsync();

            return otp;
        }

        private string GenerateOtpCode()
        {
            return RandomNumberGenerator.GetInt32(100000000, 999999999).ToString();
        }


        public async Task<User> GetUserByEmail(string email)
        {
            return await _unitOfWork.UserRepository.Get(x => x.Email == email).FirstOrDefaultAsync();
        }

        public async Task<bool> ValidateCredentials(string userLogin, string oneTimePassword)
        {
            try
            {
                var user = await _unitOfWork.UserRepository
                    .Get(x => x.Email.ToLower() == userLogin.ToLower()).FirstOrDefaultAsync();

                if (user == null)
                {
                    return false;
                }

                var otp = await _unitOfWork.GetRepository<Otp>()
                    .Get(x => x.UserId == user.Id
                    && x.Password == oneTimePassword
                    && x.ExpireDate > DateTime.UtcNow)
                    .SingleOrDefaultAsync();

                if (otp == null)
                    return false;

                // Delete OTP after successful validation to prevent reuse
                await _unitOfWork.GetRepository<Otp>().Delete(otp.Id);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
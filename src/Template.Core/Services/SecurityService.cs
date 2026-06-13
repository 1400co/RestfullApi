using Microsoft.EntityFrameworkCore;
using Template.Core.Entities;
using Template.Core.Interfaces;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Template.Core.Services
{
    public class SecurityService(IUnitOfWork unitOfWork) : ISecurityService
    {

        public async Task<Otp> GetOneTimePassword(Guid userId)
        {
            // Buscar un OTP existente para el usuario
            var otp = (await unitOfWork.GetRepository<Otp>()
                .Get(x => x.UserId == userId && x.ExpireDate > DateTime.UtcNow)
                .SingleOrDefaultAsync().ConfigureAwait(false))!;

            // Si existe un OTP y no ha expirado, devolverlo
            if (otp != null)
            {
                return otp;
            }

            // Si no existe un OTP o ha expirado, generar uno nuevo
            otp = new Otp
            {
                UserId = userId,
                Password = GenerateOtpCode(), // Mï¿½todo para generar el cï¿½digo OTP
                ExpireDate = DateTime.UtcNow.AddMinutes(5), // Expira en 5 minutos
                CreatedAt = DateTime.UtcNow,
                Responsable = "System" // Puedes ajustar esto segï¿½n tus necesidades
            };

            // Guardar el nuevo OTP en la base de datos
            await unitOfWork.GetRepository<Otp>().AddAsync(otp).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            return otp;
        }

        private string GenerateOtpCode()
        {
            return RandomNumberGenerator.GetInt32(100000000, 999999999).ToString();
        }


        public async Task<User?> GetUserByEmail(string email)
        {
            return await unitOfWork.UserRepository.Get(x => x.Email == email).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<bool> ValidateCredentials(string userLogin, string oneTimePassword)
        {
            try
            {
                var user = await unitOfWork.UserRepository
                    .Get(x => x.Email.ToLower() == userLogin.ToLower()).FirstOrDefaultAsync().ConfigureAwait(false);

                if (user == null)
                {
                    return false;
                }

                var otp = await unitOfWork.GetRepository<Otp>()
                    .Get(x => x.UserId == user.Id
                    && x.Password == oneTimePassword
                    && x.ExpireDate > DateTime.UtcNow)
                    .SingleOrDefaultAsync().ConfigureAwait(false);

                if (otp == null)
                    return false;

                // Delete OTP after successful validation to prevent reuse
                await unitOfWork.GetRepository<Otp>().Delete(otp.Id).ConfigureAwait(false);
                await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
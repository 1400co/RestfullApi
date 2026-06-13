using Microsoft.Extensions.Options;
using Template.Infrastructure.Interfaces;
using Template.Infrastructure.Options;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Template.Infrastructure.Services
{
    public class PasswordService(IOptions<PasswordOptions> options) : IPasswordService
    {

        public bool Check(string hash, string password)
        {
            var parts = hash.Split('.');
            if (parts.Length != 3)
            {
                throw new FormatException("Unexpected hash format");
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            var keyToCheck = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, iterations, HashAlgorithmName.SHA256, options.Value.KeySize);
            return keyToCheck.SequenceEqual(key);
        }

        public string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(options.Value.SaltSize);
            var keyBytes = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, options.Value.Iterations, HashAlgorithmName.SHA256, options.Value.KeySize);
            return $"{options.Value.Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(keyBytes)}";
        }
    }
}

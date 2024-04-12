using SocialMedia.Core.Dtos;
using System;

namespace SocialMedia.Core.Entities
{
    public class PasswordUpdate : BaseDto
    {
        public Guid PasswordRecoveryToken { get; set; }
        public string Password { get; set; }
    }
}

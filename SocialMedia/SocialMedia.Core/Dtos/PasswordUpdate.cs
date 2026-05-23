using SocialMedia.Core.Dtos;
using System;

namespace SocialMedia.Core.Entities
{
    public record PasswordUpdate : BaseDto
    {
        public Guid PasswordRecoveryToken { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}

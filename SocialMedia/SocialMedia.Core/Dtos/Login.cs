using SocialMedia.Core.Dtos;
using System;

namespace SocialMedia.Core.Entities
{
    public record Login : BaseDto
    {
        public string Email { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
}

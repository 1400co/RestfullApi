using SocialMedia.Core.Dtos;
using System;

namespace SocialMedia.Core.Entities
{
    public class Login : BaseDto
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}

using SocialMedia.Core.Dtos;
using System;

namespace SocialMedia.Core.DTOs
{
    public class OtpDto : BaseDto
    {
        public Guid UserId { get; set; }
        public UserDto User { get; set; }
        public string Password { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}

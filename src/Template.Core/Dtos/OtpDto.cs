using Template.Core.Dtos;
using System;

namespace Template.Core.DTOs
{
    public record OtpDto : BaseDto
    {
        public Guid UserId { get; set; }
        public UserDto? User { get; set; }
        public string Password { get; set; } = string.Empty;
        public DateTime? ExpireDate { get; set; }
    }
}

using Template.Core.Dtos;
using System;

namespace Template.Core.Entities
{
    public record PasswordUpdate : BaseDto
    {
        public Guid PasswordRecoveryToken { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}

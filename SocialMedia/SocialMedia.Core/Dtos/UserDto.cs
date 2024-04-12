using SocialMedia.Core.Entities;
using SocialMedia.Core.Enumerations;
using System;

namespace SocialMedia.Core.Dtos
{
    public partial class UserDto : BaseEntity
    {
        public UserDto()
        {

        }

        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BornDate { get; set; }
        public string Phone { get; set; }
        public string Dni { get; set; }
        public bool IsActive { get; set; }
        public Subscription Subscription { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; } = string.Empty;
        public virtual string  Roles { get; set; } = string.Empty;
    }
}

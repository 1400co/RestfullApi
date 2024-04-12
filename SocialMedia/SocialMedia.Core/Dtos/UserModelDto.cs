using SocialMedia.Core.Entities;
using SocialMedia.Core.Enumerations;
using System.Collections.Generic;

namespace SocialMedia.Core.Dtos
{
    public partial class UserModelDto : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public Subscription Subscription { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; } = new List<string>();

    }
}

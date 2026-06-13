using Template.Core.Entities;
using Template.Core.Enumerations;
using System.Collections.Generic;

namespace Template.Core.Dtos
{
    public partial class UserModelDto : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Subscription Subscription { get; set; }
        public string UserName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = [];

    }
}

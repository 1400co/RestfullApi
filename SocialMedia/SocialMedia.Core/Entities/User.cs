﻿using SocialMedia.Core.Dtos;
using SocialMedia.Core.Enumerations;
using System;
using System.Collections.Generic;

namespace SocialMedia.Core.Entities
{
    public partial class User : BaseEntity
    {
        public User()
        {
            Comment = new HashSet<CommentDto>();
            Post = new HashSet<Post>();
            Security = new HashSet<Security>();
            UserInRoles = new HashSet<UserInRolesDto>();
            PasswordRecovery = new HashSet<PasswordRecoveryDto>();
        }

        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime BornDate { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public Subscription Subscription { get; set; }

        public virtual ICollection<CommentDto> Comment { get; set; }
        public virtual ICollection<Post> Post { get; set; }
        public virtual ICollection<Security> Security { get; set; }
        public virtual ICollection<UserInRolesDto> UserInRoles { get; set; }
        public virtual ICollection<PasswordRecoveryDto> PasswordRecovery { get; set; }
        
    }
}
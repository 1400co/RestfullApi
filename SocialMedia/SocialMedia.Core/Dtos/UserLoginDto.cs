﻿using SocialMedia.Core.Dtos;
using System;

namespace SocialMedia.Core.Entities
{
    public class UserLoginDto : BaseDto
    {
        public string User { get; set; }
        public string Password { get; set; }
        public virtual Guid  UserId { get; set; }
    }
}

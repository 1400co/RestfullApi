﻿using SocialMedia.Core.Dtos;
using System;
using System.Collections.Generic;

namespace SocialMedia.Core.Entities
{
    public partial class Post : BaseEntity
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Date = DateTime.Now;
        }

        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public virtual UserDto User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}

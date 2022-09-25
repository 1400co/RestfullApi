using System;
using System.Collections.Generic;

namespace SocialMedia.Core.Entities
{
    public partial class User
    {
        public User()
        {
            Comment = new HashSet<Comment>();
            Post = new HashSet<Post>();
        }

        public int UserId { get; set; }
        public string Names { get; set; }
        public string LastNames { get; set; }
        public string Email { get; set; }
        public DateTime BornDate { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Post> Post { get; set; }
    }
}

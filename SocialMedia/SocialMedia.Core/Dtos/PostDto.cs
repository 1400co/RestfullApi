using System;

namespace SocialMedia.Core.Dtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime? Date { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
    }
}

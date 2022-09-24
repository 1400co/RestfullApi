using SocialMedia.Core.Entities;

namespace SocialMedia.Infrastructure.Repositories
{
    public class PostRepository
    {
        public PostRepository()
        {

        }

        public IEnumerable<Post> GetPosts()
        {
            var posts = Enumerable.Range(1, 10).Select(x => new Post()
            {
                PostId = x,
                Date = DateTime.UtcNow,
                Description = $"Descripcion {x}",
                Image ="http://unaimagen.png",
                UserId = x * 2
            });

            return posts;
        }
    }
}

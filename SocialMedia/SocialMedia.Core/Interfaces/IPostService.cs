using SocialMedia.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IPostService
    {
        Task InsertPost(Post post);
        Task<bool> UpdatePost(Post post);
        Task<Post> GetPost(int id);
        Task<IEnumerable<Post>> GetPosts();
        Task<bool> DeletePost(int id);
    }
}
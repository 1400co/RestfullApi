using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IPostService
    {
        Task InsertPost(Post post);
        Task<bool> UpdatePost(Post post);
        Task<Post> GetPost(int id);
        IEnumerable<Post> GetPosts(PostQueryFilter filters);
        Task DeletePost(int id);
    }
}
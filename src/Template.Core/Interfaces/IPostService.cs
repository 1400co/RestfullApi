using Template.Core.CustomEntities;
using Template.Core.Entities;
using Template.Core.QueryFilters;
using System;
using System.Threading.Tasks;

namespace Template.Core.Interfaces
{
    public interface IPostService
    {
        Task InsertPost(Post post);
        Task<bool> UpdatePost(Post post);
        Task<Post?> GetPost(Guid id);
        Task<PagedList<Post>> GetPosts(PostQueryFilter filters);
        Task DeletePost(Guid id);
    }
}
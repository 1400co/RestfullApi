using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface ICommentService
    {
        Task DeleteComment(Guid id);
        Task<Comment> GetComment(Guid id);
        PagedList<Comment> GetComments(CommentQueryFilter filters);
        Task InsertComment(Comment comment);
        Task<bool> UpdateComment(Comment comment);
    }
}
using Template.Core.CustomEntities;
using Template.Core.Entities;
using Template.Core.QueryFilters;
using System;
using System.Threading.Tasks;

namespace Template.Core.Interfaces
{
    public interface ICommentService
    {
        Task DeleteComment(Guid id);
        Task<Comment?> GetComment(Guid id);
        Task<PagedList<Comment>> GetComments(CommentQueryFilter filters);
        Task InsertComment(Comment comment);
        Task<bool> UpdateComment(Comment comment);
    }
}
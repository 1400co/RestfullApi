using Microsoft.Extensions.Options;
using Template.Core.CustomEntities;
using Template.Core.Entities;
using Template.Core.Exceptions;
using Template.Core.Interfaces;
using Template.Core.QueryFilters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Template.Core.Services
{
    public class CommentService : GenericService<Comment>, ICommentService
    {
        public CommentService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions)
            : base(unitOfWork, paginationOptions)
        {
        }

        public async Task InsertComment(Comment comment)
        {
            var user = await _unitOfWork.UserRepository.GetById(comment.UserId).ConfigureAwait(false);
            if (user == null)
                throw new BusinessException("User doesn't exist");

            if (comment.Description.Contains("sexo"))
                throw new BusinessException("Content not allowed");

            await _unitOfWork.CommentRepository.Insert(comment).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> UpdateComment(Comment comment)
        {
            return await base.Update(comment).ConfigureAwait(false);
        }

        public async Task<Comment?> GetComment(Guid id)
        {
            return await base.Get(id).ConfigureAwait(false);
        }

        public async Task<PagedList<Comment>> GetComments(CommentQueryFilter filters)
        {
            var comments = _unitOfWork.CommentRepository.Get();

            if (filters.UserId != null)
                comments = comments.Where(x => x.UserId == filters.UserId);

            if (filters.Description != null)
                comments = comments.Where(x => x.Description.Contains(filters.Description));

            var pagedComments = await PagedList<Comment>.CreateAsync(comments, filters.PageNumber, filters.PageSize).ConfigureAwait(false);
            return pagedComments;
        }

        public async Task DeleteComment(Guid id)
        {
            await base.Delete(id).ConfigureAwait(false);
        }
    }
}

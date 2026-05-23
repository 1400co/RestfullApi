using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace SocialMedia.Core.Services
{
    public class CommentService(IUnitOfWork unitOfWork) : ICommentService
    {

        public async Task InsertComment(Comment comment)
        {
            var user = await unitOfWork.UserRepository.GetById(comment.UserId).ConfigureAwait(false);
            if (user == null)
                throw new BusinessException("User doesn't exist");

            if (comment.Description.Contains("sexo"))
                throw new BusinessException("Content not allowed");

            await unitOfWork.CommentRepository.Insert(comment).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> UpdateComment(Comment comment)
        {
            var existingComment = await unitOfWork.CommentRepository.GetById(comment.Id).ConfigureAwait(false);
            if (existingComment == null)
                throw new BusinessException("Comment doesn't exist");

            comment.CopyPropertiesTo(existingComment);

            await unitOfWork.CommentRepository.Update(existingComment).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public async Task<Comment?> GetComment(Guid id)
        {
            return await unitOfWork.CommentRepository.GetById(id).ConfigureAwait(false);
        }

        public async Task<PagedList<Comment>> GetComments(CommentQueryFilter filters)
        {
            var comments = unitOfWork.CommentRepository.Get();

            // Set default pagination values or use provided filters
            filters.PageNumber = filters.PageNumber;
            filters.PageSize = filters.PageSize;

            // Additional filtering logic if necessary
            if (filters.UserId != null)
            {
                comments = comments.Where(x => x.UserId == filters.UserId);
            }

            if (filters.Description != null)
            {
                comments = comments.Where(x => x.Description.Contains(filters.Description));
            }

            var pagedComments = await PagedList<Comment>.CreateAsync(comments, filters.PageNumber, filters.PageSize).ConfigureAwait(false);

            return pagedComments;
        }

        public async Task DeleteComment(Guid id)
        {
            await unitOfWork.CommentRepository.Delete(id).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
    }

}

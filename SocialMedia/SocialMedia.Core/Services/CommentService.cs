using AutoMapper;
using Microsoft.Extensions.Options;
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
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;

        public CommentService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = paginationOptions.Value;
        }

        public async Task InsertComment(Comment comment)
        {
            var user = await _unitOfWork.UserRepository.GetById(comment.UserId);
            if (user == null)
                throw new BusinessException("User doesn't exist");

            if (comment.Description.Contains("sexo"))
                throw new BusinessException("Content not allowed");

            await _unitOfWork.CommentRepository.Insert(comment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateComment(Comment comment)
        {
            var existingComment = await _unitOfWork.CommentRepository.GetById(comment.Id);
            if (existingComment == null)
                throw new BusinessException("Comment doesn't exist");

            comment.CopyPropertiesTo(existingComment);

            await _unitOfWork.CommentRepository.Update(existingComment);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<Comment> GetComment(Guid id)
        {
            return await _unitOfWork.CommentRepository.GetById(id);
        }

        public async Task<PagedList<Comment>> GetComments(CommentQueryFilter filters)
        {
            var comments = _unitOfWork.CommentRepository.Get();

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

            var pagedComments = await PagedList<Comment>.CreateAsync(comments, filters.PageNumber, filters.PageSize);

            return pagedComments;
        }

        public async Task DeleteComment(Guid id)
        {
            await _unitOfWork.CommentRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }

}

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

    public class PostService : IPostService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly PaginationOptions _paginationOptions;
        public PostService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = paginationOptions.Value;
        }

        public async Task InsertPost(Post post)
        {
            var user = await _unitOfWork.UserRepository.GetById(post.UserId);
            if (user == null)
                throw new BusinessException("User doesnt exist");

            if (post.Description.Contains("sexo"))
                throw new BusinessException("Content not allowed");

            var userPosts = await _unitOfWork.PostRepository.GetPostsByUser(post.UserId);
            if (userPosts.Count() < 10)
            {
                var lastPost = userPosts.OrderByDescending(x => x.Date).FirstOrDefault();
                if (lastPost != null && (System.DateTime.Now - lastPost.Date).TotalDays < 7)
                {
                    throw new BusinessException("you cant push any posts jet.");
                }
            }

            await _unitOfWork.PostRepository.Insert(post);
        }

        public async Task<bool> UpdatePost(Post post)
        {
            var existingPost = await _unitOfWork.PostRepository.GetById(post.Id, u => u.User);
            existingPost.Image = post.Image;
            existingPost.Description = post.Description;

            await _unitOfWork.PostRepository.Update(existingPost);
            return true;
        }

        public async Task<Post> GetPost(Guid id)
        {
            return await _unitOfWork.PostRepository.GetById(id, u => u.User);
        }

        public PagedList<Post> GetPosts(PostQueryFilter filters)
        {
            var posts = _unitOfWork.PostRepository.Get(u => u.User);

            filters.PageNumber = _paginationOptions.DefaultPageNumber;
            filters.PageSize = _paginationOptions.DefaultPageSize;

            if (filters.UserId != null)
            {
                posts = posts.Where(x => x.UserId == filters.UserId);
            }

            if (filters.Date != null)
            {
                posts = posts.Where(x => x.Date.ToShortDateString() == ((DateTime)filters.Date).ToShortDateString());
            }

            if (filters.Description != null)
            {
                posts = posts.Where(x => x.Description.ToLower().Contains(filters.Description.ToLower()));
            }

            var pagedPosts = PagedList<Post>.Create(posts, filters.PageSize, filters.PageSize);

            return pagedPosts;
        }

        public async Task DeletePost(Guid id)
        {
            await _unitOfWork.PostRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

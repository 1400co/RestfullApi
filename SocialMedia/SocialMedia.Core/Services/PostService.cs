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
    public class PostService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions) : IPostService
    {

        public async Task InsertPost(Post post)
        {
            var user = await unitOfWork.UserRepository.GetById(post.UserId).ConfigureAwait(false);
            if (user == null)
                throw new BusinessException("User doesnt exist");

            if (post.Description.Contains("sexo"))
                throw new BusinessException("Content not allowed");

            var userPosts = await unitOfWork.PostRepository.GetPostsByUser(post.UserId).ConfigureAwait(false);
            if (userPosts.Count() < 10)
            {
                var lastPost = userPosts.OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                if (lastPost != null && (System.DateTime.UtcNow - lastPost.CreatedAt).TotalDays < 7)
                {
                    throw new BusinessException("you cant push any posts jet.");
                }
            }

            await unitOfWork.PostRepository.Insert(post).ConfigureAwait(false);
        }

        public async Task<bool> UpdatePost(Post post)
        {
            var existingPost = await unitOfWork.PostRepository.GetById(post.Id, u => u.User).ConfigureAwait(false);
            if (existingPost == null)
                throw new BusinessException("Post doesn't exist");

            post.CopyPropertiesTo(existingPost);

            await unitOfWork.PostRepository.Update(existingPost).ConfigureAwait(false);
            return true;
        }

        public async Task<Post?> GetPost(Guid id)
        {
            return await unitOfWork.PostRepository.GetById(id, u => u.User).ConfigureAwait(false);
        }

        public async Task<PagedList<Post>> GetPosts(PostQueryFilter filters)
        {
            var posts = unitOfWork.PostRepository.Get(null, u => u.User);

            if (filters.PageNumber == 0) filters.PageNumber = paginationOptions.Value.DefaultPageNumber;
            if (filters.PageSize == 0) filters.PageSize = paginationOptions.Value.DefaultPageSize;

            if (filters.UserId != null)
            {
                posts = posts.Where(x => x.UserId == filters.UserId);
            }

            if (filters.Description != null)
            {
                posts = posts.Where(x => x.Description.ToLower().Contains(filters.Description.ToLower()));
            }

            var pagedPosts = await PagedList<Post>.CreateAsync(posts, filters.PageNumber, filters.PageSize).ConfigureAwait(false);

            return pagedPosts;
        }

        public async Task DeletePost(Guid id)
        {
            await unitOfWork.PostRepository.Delete(id).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}

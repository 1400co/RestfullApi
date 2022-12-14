using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{

    public class PostService : IPostService
    {
    
        private readonly IUnitOfWork _unitOfWork;
        public PostService( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task InsertPost(Post post)
        {
            var user = _unitOfWork.UserRepository.GetById(post.UserId);
            if (user == null)
                throw new System.Exception("User doesnt exist");

            if (post.Description.Contains("sexo"))
                throw new System.Exception("Content not allowed");

            await _unitOfWork.PostRepository.Insert(post);
        }

        public async Task UpdatePost(Post post)
        {
            await _unitOfWork.PostRepository.Update(post);
        }

        public async Task<Post> GetPost(int id)
        {
            return await _unitOfWork.PostRepository.GetById(id);
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await _unitOfWork.PostRepository.GetByAll();
        }

        public async Task DeletePost(int id)
        {
            await _unitOfWork.PostRepository.Delete(id);
        }
    }
}

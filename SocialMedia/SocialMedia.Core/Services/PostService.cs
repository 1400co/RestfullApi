using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{

    public class PostService : IPostService
    {
        private readonly IPostService postRepository;
        private readonly IUserRepository userRepository;

        public PostService(IPostService postRepository, IUserRepository userRepository)
        {
            this.postRepository = postRepository;
            this.userRepository = userRepository;
        }

        public async Task InsertPost(Post post)
        {
            var user = userRepository.GetUser(post.UserId);
            if (user == null)
                throw new System.Exception("User doesnt exist");

            if (post.Description.Contains("sexo"))
                throw new System.Exception("Content not allowed");

            await postRepository.InsertPost(post);
        }

        public async Task<bool> UpdatePost(Post post)
        {
            return await postRepository.UpdatePost(post);
        }

        public async Task<Post> GetPost(int id)
        {
            return await postRepository.GetPost(id);
        }

        public async Task<IEnumerable<Post>> GetPosts()
        {
            return await postRepository.GetPosts();
        }

        public async Task<bool> DeletePost(int id)
        {
            return await postRepository.DeletePost(id);
        }
    }
}

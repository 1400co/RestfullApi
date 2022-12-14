using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SocialMediaContext socialMediaContext;
        private readonly IRepository<Post> postRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Comment> commentRepository;

        public UnitOfWork(SocialMediaContext socialMediaContext)
        {
            this.socialMediaContext = socialMediaContext;
        }
        public IRepository<Post> PostRepository => PostRepository ?? new BaseRepository<Post>(socialMediaContext);

        public IRepository<User> UserRepository => userRepository ?? new BaseRepository<User>(socialMediaContext);

        public IRepository<Comment> CommentRepository => CommentRepository ?? new BaseRepository<Comment>(socialMediaContext);

        public void Dispose()
        {
            if(socialMediaContext != null)
                socialMediaContext.Dispose();
        }

        public void SaveChanges()
        {
            socialMediaContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
          await  socialMediaContext.SaveChangesAsync();
        }
    }
}

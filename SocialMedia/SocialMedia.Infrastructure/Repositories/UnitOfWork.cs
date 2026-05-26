using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SocialMediaContext socialMediaContext;

        public UnitOfWork(SocialMediaContext socialMediaContext)
        {
            this.socialMediaContext = socialMediaContext;
        }

        public IPostRepository PostRepository => new PostRepository(socialMediaContext);
        public IRepository<User> UserRepository => new BaseRepository<User>(socialMediaContext);
        public IRepository<Comment> CommentRepository => new BaseRepository<Comment>(socialMediaContext);
        public IRepository<RolModule> RolModuleRepository => new BaseRepository<RolModule>(socialMediaContext);
        public IRepository<Modules> ModuleRepository => new BaseRepository<Modules>(socialMediaContext);
        public IRepository<Otp> OtpRepository => new BaseRepository<Otp>(socialMediaContext);
        public IRepository<Cervezas> CervezasRepository => new BaseRepository<Cervezas>(socialMediaContext);

        public void Dispose()
        {
            socialMediaContext?.Dispose();
        }

        public void SaveChanges()
        {
            socialMediaContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await socialMediaContext.SaveChangesAsync();
        }

        public IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            return new BaseRepository<T>(socialMediaContext);
        }
    }
}


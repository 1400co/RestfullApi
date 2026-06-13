using Template.Core.Entities;
using Template.Core.Interfaces;
using Template.Infrastructure.Data;
using System.Threading.Tasks;

namespace Template.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TemplateContext TemplateContext;

        public UnitOfWork(TemplateContext TemplateContext)
        {
            this.TemplateContext = TemplateContext;
        }

        public IPostRepository PostRepository => new PostRepository(TemplateContext);
        public IRepository<User> UserRepository => new BaseRepository<User>(TemplateContext);
        public IRepository<Comment> CommentRepository => new BaseRepository<Comment>(TemplateContext);
        public IRepository<RolModule> RolModuleRepository => new BaseRepository<RolModule>(TemplateContext);
        public IRepository<Modules> ModuleRepository => new BaseRepository<Modules>(TemplateContext);
        public IRepository<Otp> OtpRepository => new BaseRepository<Otp>(TemplateContext);
        public IRepository<Cervezas> CervezasRepository => new BaseRepository<Cervezas>(TemplateContext);

        public void Dispose()
        {
            TemplateContext?.Dispose();
        }

        public void SaveChanges()
        {
            TemplateContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await TemplateContext.SaveChangesAsync();
        }

        public IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            return new BaseRepository<T>(TemplateContext);
        }
    }
}


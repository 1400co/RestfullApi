using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SocialMediaContext socialMediaContext;
        private readonly IPostRepository postRepository;
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Comment> commentRepository;
        private readonly IRepository<PasswordRecovery> passwordRecoveryRepository;
        private readonly IRepository<Roles> rolesRepository;
        private readonly IRepository<RolModule> rolModuleRepository;
        private readonly IRepository<UserInRoles> userInRolesRepository;
        private readonly IRepository<UserLogin> userLoginRepository;
        private readonly IRepository<Modules> moduleRepository;
        private readonly ISecurityRepository _securityRepository;

        public UnitOfWork(SocialMediaContext socialMediaContext)
        {
            this.socialMediaContext = socialMediaContext;
        }
        public IPostRepository PostRepository => postRepository ?? new PostRepository(socialMediaContext);

        public IRepository<User> UserRepository => userRepository ?? new BaseRepository<User>(socialMediaContext);
        public IRepository<PasswordRecovery> PasswordRecoveryRepository => passwordRecoveryRepository ?? new BaseRepository<PasswordRecovery>(socialMediaContext);
        public IRepository<Comment> CommentRepository => commentRepository ?? new BaseRepository<Comment>(socialMediaContext);
        public IRepository<Roles> RolesRepository => rolesRepository ?? new BaseRepository<Roles>(socialMediaContext);
        public IRepository<RolModule> RolModuleRepository => rolModuleRepository ?? new BaseRepository<RolModule>(socialMediaContext);
        public IRepository<UserInRoles> UserInRolesRepository => userInRolesRepository ?? new BaseRepository<UserInRoles>(socialMediaContext);
        public IRepository<UserLogin> UserLoginRepository => userLoginRepository ?? new BaseRepository<UserLogin>(socialMediaContext);
        public IRepository<Modules> ModuleRepository => moduleRepository ?? new BaseRepository<Modules>(socialMediaContext);

        public ISecurityRepository SecurityRepository => _securityRepository ?? new SecurityRepository(socialMediaContext);

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

        public IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            return new BaseRepository<T>(socialMediaContext);
        }

    }
}


using SocialMedia.Core.Entities;
using System;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository PostRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<Comment> CommentRepository { get; }
        IRepository<PasswordRecovery> PasswordRecoveryRepository { get; }
        IRepository<Roles> RolesRepository { get; }
        IRepository<RolModule> RolModuleRepository { get; }
        IRepository<UserInRoles> UserInRolesRepository { get; }
        ISecurityRepository SecurityRepository { get; }
        void SaveChanges();
        Task SaveChangesAsync();

    }
}


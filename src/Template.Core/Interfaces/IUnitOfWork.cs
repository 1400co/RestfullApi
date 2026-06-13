using Template.Core.Entities;
using System;
using System.Threading.Tasks;

namespace Template.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPostRepository PostRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<Comment> CommentRepository { get; }
        IRepository<RolModule> RolModuleRepository { get; }
        IRepository<Modules> ModuleRepository { get; }
        IRepository<Otp> OtpRepository { get; }
        IRepository<Cervezas> CervezasRepository { get; }
        void SaveChanges();
        Task SaveChangesAsync();
        IRepository<T> GetRepository<T>() where T : BaseEntity;

    }
}


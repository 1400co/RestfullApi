using Template.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Template.Core.Interfaces
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> GetPostsByUser(Guid id);
    }
}

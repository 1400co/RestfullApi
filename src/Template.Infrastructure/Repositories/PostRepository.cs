using Microsoft.EntityFrameworkCore;
using Template.Core.Entities;
using Template.Core.Interfaces;
using Template.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template.Infrastructure.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(TemplateContext TemplateContext) : base(TemplateContext)
        {
        }

        public async Task<IEnumerable<Post>> GetPostsByUser(Guid userId)
        {
            return await  entities.Where(p => p.UserId == userId).ToListAsync();
        }
    }
}

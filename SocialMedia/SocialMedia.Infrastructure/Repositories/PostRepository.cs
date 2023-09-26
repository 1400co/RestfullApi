using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(SocialMediaContext socialMediaContext) : base(socialMediaContext)
        {
        }

        public async Task<IEnumerable<Post>> GetPostsByUser(Guid userId)
        {
            return await  entities.Where(p => p.UserId == userId).ToListAsync();
        }
    }
}

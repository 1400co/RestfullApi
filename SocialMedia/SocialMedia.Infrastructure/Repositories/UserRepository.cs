
using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SocialMediaContext _socialMediaContext;
        public UserRepository(SocialMediaContext socialMediaContext)
        {
            _socialMediaContext = socialMediaContext;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var posts = await this._socialMediaContext.Users.ToListAsync();

            return posts;
        }

        public async Task<User> GetUser(int id)
        {
            var post = await _socialMediaContext.Users.FirstOrDefaultAsync(x => x.UserId == id);

            return post;
        }


    }
}
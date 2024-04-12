using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class SecurityRepository : BaseRepository<Security>, ISecurityRepository
    {
        public SecurityRepository(SocialMediaContext context) : base(context) { }

        public async Task<Security> GetLoginByCredentials(UserLoginDto login)
        {
            return await entities.Include(x => x.User).FirstOrDefaultAsync(x => x.UserName == login.User);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly SocialMediaContext socialMediaContext;
        private readonly DbSet<T> entities;
        public BaseRepository(SocialMediaContext socialMediaContext)
        {
            this.socialMediaContext = socialMediaContext;
            entities = socialMediaContext.Set<T>();
        }

        public async Task<IEnumerable<T>> GetByAll()
        {
            return await entities.ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await entities.FindAsync(id);
        }

        public async Task Insert(T entity)
        {
            entities.Add(entity);
            socialMediaContext.SaveChanges();
        }

        public async Task Update(T entity)
        {
            entities.Update(entity);
            socialMediaContext.SaveChanges();
        }

        public async Task Delete(int id)
        {
            T entity = await GetById(id);
            entities.Remove(entity);
            socialMediaContext.SaveChanges();
        }
    }
}

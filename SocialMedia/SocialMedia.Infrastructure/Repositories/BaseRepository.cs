using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly SocialMediaContext socialMediaContext;
        protected readonly DbSet<T> entities;
        public BaseRepository(SocialMediaContext socialMediaContext)
        {
            this.socialMediaContext = socialMediaContext;
            entities = socialMediaContext.Set<T>();
        }

        public IEnumerable<T> GetByAll()
        {
            return entities.AsEnumerable();
        }

        public async Task<T> GetById(int id)
        {
            return await entities.FindAsync(id);
        }

        public async Task Insert(T entity)
        {
            await entities.AddAsync(entity);
        }

        public async void Update(T entity)
        {
            entities.Update(entity);
        }

        public async Task Delete(int id)
        {
            T entity = await GetById(id);
            entities.Remove(entity);
        }

        public async Task Add(T entity)
        {
            await entities.AddAsync(entity);
        }
    }
}

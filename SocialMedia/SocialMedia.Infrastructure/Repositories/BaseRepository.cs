using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
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

        public IQueryable<T> Get(params Expression<Func<T, object>>[] includes)
        {
            var query = entities.AsQueryable();

            if (includes != null)
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

            return query;
        }

        public async Task<T> GetById(Guid id, params Expression<Func<T, object>>[] includes)
        {
            var query = entities.AsQueryable();

            if (includes != null)
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

       

        public async Task Insert(T entity)
        {
            await entities.AddAsync(entity);
        }

        public async Task<List<T>> Insert(List<T> entities)
        {
            await this.entities.AddRangeAsync(entities);
            await socialMediaContext.SaveChangesAsync(); // You might want to save the changes after adding.
            return entities;
        }

        public async void Update(T entity)
        {
            entities.Update(entity);
        }

        public async Task Update(List<T> entities)
        {
            this.entities.UpdateRange(entities);
            await socialMediaContext.SaveChangesAsync(); // You might want to save the changes after updating.
        }

        public async Task Delete(Guid id)
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

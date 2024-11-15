using Microsoft.EntityFrameworkCore;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

       public IEnumerable<T> GetByAll(
            Expression<Func<T, bool>> additionalConditions = null,
            params Expression<Func<T, object>>[] includes)
                {
            var query = entities.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (additionalConditions != null)
            {
                query = query.Where(additionalConditions);
            }

            return query.AsEnumerable();
        }


        public IQueryable<T> Get(Expression<Func<T, bool>> additionalConditions = null, params Expression<Func<T, object>>[] includes)
        {
            var query = entities.AsQueryable();

            if (includes != null)
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }

            if (additionalConditions != null)
            {
                query = query.Where(additionalConditions);
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


        /// <summary>
        /// Insert with Commit
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> Insert(T entity)
        {
            await entities.AddAsync(entity);
            await socialMediaContext.SaveChangesAsync();
            return entity;  

        }

        public async Task<List<T>> Insert(List<T> entities)
        {
            await this.entities.AddRangeAsync(entities);
            await socialMediaContext.SaveChangesAsync(); // You might want to save the changes after adding.
            return entities;
        }

        public async Task Update(T entity)
        {
            entities.Update(entity);
            socialMediaContext.Entry(entity).State = EntityState.Modified;
            await socialMediaContext.SaveChangesAsync();
        }

        public async Task Update(List<T> entities)
        {
            foreach (var entity in entities)
            {
                this.entities.Attach(entity);
                socialMediaContext.Entry(entity).State = EntityState.Modified;
            }
            await socialMediaContext.SaveChangesAsync(); // You might want to save the changes after updating.
        }

        public async Task Delete(Guid id)
        {
            T entity = await GetById(id);
            entities.Remove(entity);
        }

        public void Detach(T entity)
        {
            socialMediaContext.Entry(entity).State = EntityState.Detached;
        }

        /// <summary>
        /// Add with session, no commit.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task AddAsync(T entity)
        {
            await entities.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await this.entities.AddRangeAsync(entities);
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            this.entities.UpdateRange(entities);
        }
    }
}

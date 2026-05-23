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

        private IQueryable<T> ApplyIncludes(IQueryable<T> query, Expression<Func<T, object>>[] includes)
        {
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query;
        }

        public IQueryable<T> Get(Expression<Func<T, bool>>? additionalConditions = null, params Expression<Func<T, object>>[] includes)
        {
            var query = entities.AsQueryable();

            if (additionalConditions != null)
            {
                query = query.Where(additionalConditions);
            }

            return ApplyIncludes(query, includes);
        }

        public IQueryable<T> GetAsNoTracking(Expression<Func<T, bool>>? additionalConditions = null, params Expression<Func<T, object>>[] includes)
        {
            return Get(additionalConditions, includes).AsNoTracking();
        }

        public async Task<T?> GetById(Guid id, params Expression<Func<T, object>>[] includes)
        {
            var query = entities.AsQueryable();
            query = ApplyIncludes(query, includes);
            return await query.FirstOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);
        }

        public async Task<T?> GetByIdAsNoTracking(Guid id, params Expression<Func<T, object>>[] includes)
        {
            var query = entities.AsQueryable().AsNoTracking();
            query = ApplyIncludes(query, includes);
            return await query.FirstOrDefaultAsync(e => e.Id == id).ConfigureAwait(false);
        }

        public async Task<T> Insert(T entity)
        {
            await entities.AddAsync(entity).ConfigureAwait(false);
            return entity;
        }

        public async Task<List<T>> Insert(List<T> entities)
        {
            await this.entities.AddRangeAsync(entities).ConfigureAwait(false);
            return entities;
        }

        public async Task Update(T entity)
        {
            entities.Update(entity);
        }

        public async Task Update(List<T> entities)
        {
            foreach (var entity in entities)
            {
                this.entities.Attach(entity);
                socialMediaContext.Entry(entity).State = EntityState.Modified;
            }
        }

        public async Task Delete(Guid id)
        {
            var entity = await GetById(id).ConfigureAwait(false);
            if (entity is not null)
                entities.Remove(entity);
        }

        public Task Delete(T entity)
        {
            entities.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate == null
                ? await entities.AnyAsync()
                : await entities.AnyAsync(predicate).ConfigureAwait(false);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
        {
            return predicate == null
                ? await entities.CountAsync()
                : await entities.CountAsync(predicate).ConfigureAwait(false);
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>>? predicate = null, params Expression<Func<T, object>>[] includes)
        {
            var query = entities.AsQueryable();
            query = ApplyIncludes(query, includes);

            return predicate == null
                ? await query.FirstOrDefaultAsync()
                : await query.FirstOrDefaultAsync(predicate).ConfigureAwait(false);
        }

        public void Detach(T entity)
        {
            socialMediaContext.Entry(entity).State = EntityState.Detached;
        }

        public async Task AddAsync(T entity)
        {
            await entities.AddAsync(entity).ConfigureAwait(false);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await this.entities.AddRangeAsync(entities).ConfigureAwait(false);
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            this.entities.UpdateRange(entities);
        }
    }
}

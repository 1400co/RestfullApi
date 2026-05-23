using Microsoft.Extensions.Options;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class GenericService<T>(IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions) : IGenericService<T> where T : BaseEntity, new()
    {

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var existingEntities = unitOfWork.GetRepository<T>().Get(null, includes);
            return existingEntities;
        }

        public async Task Insert(T entity)
        {
            var existingEntity = await unitOfWork.GetRepository<T>().GetById(entity.Id).ConfigureAwait(false);
            if (existingEntity != null)
                throw new BusinessException($"{typeof(T).Name} already exists");

            await unitOfWork.GetRepository<T>().Insert(entity).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> Update(T entity)
        {
            var existingEntity = await unitOfWork.GetRepository<T>().GetById(entity.Id).ConfigureAwait(false);
            if (existingEntity == null)
                throw new BusinessException($"{typeof(T).Name} doesn't exist");

            entity.CopyPropertiesTo(existingEntity);

            await unitOfWork.GetRepository<T>().Update(existingEntity).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public async Task<T?> Get(Guid id)
        {
            return await unitOfWork.GetRepository<T>().GetById(id).ConfigureAwait(false);
        }

        public async Task<PagedList<T>> Get(GeneralQueryFilter filters, params Expression<Func<T, object>>[] includes)
        {
            var entities = unitOfWork.GetRepository<T>().Get(null, includes);

            if (!string.IsNullOrWhiteSpace(filters.Filter))
            {
                var condiciones = typeof(T).GetProperties()
                    .Where(prop => prop.PropertyType == typeof(string))
                    .Select(prop => $"{prop.Name}.Contains(@0)");

                string predicado = string.Join(" || ", condiciones);
                if (!string.IsNullOrEmpty(predicado))
                {
                    entities = entities.Where(predicado, filters.Filter);
                }
            }

            if (filters.PageNumber == 0) filters.PageNumber = paginationOptions.Value.DefaultPageNumber;
            if (filters.PageSize == 0) filters.PageSize = paginationOptions.Value.DefaultPageSize;

            var pagedEntities = await PagedList<T>.CreateAsync(entities, filters.PageNumber, filters.PageSize).ConfigureAwait(false);

            return pagedEntities;
        }

        public async Task Delete(Guid id)
        {
            await unitOfWork.GetRepository<T>().Delete(id).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}

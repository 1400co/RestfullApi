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
    public class GenericService<T> : IGenericService<T> where T : BaseEntity, new()
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly PaginationOptions _paginationOptions;

        public GenericService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions)
        {
            _unitOfWork = unitOfWork;
            _paginationOptions = paginationOptions.Value;
        }

        public IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var existingEntities = _unitOfWork.GetRepository<T>().Get(null, includes);
            return existingEntities;
        }

        public async Task Insert(T entity)
        {
            var existingEntity = await _unitOfWork.GetRepository<T>().GetById(entity.Id).ConfigureAwait(false);
            if (existingEntity != null)
                throw new BusinessException($"{typeof(T).Name} already exists");

            await _unitOfWork.GetRepository<T>().Insert(entity).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> Update(T entity)
        {
            var existingEntity = await _unitOfWork.GetRepository<T>().GetById(entity.Id).ConfigureAwait(false);
            if (existingEntity == null)
                throw new BusinessException($"{typeof(T).Name} doesn't exist");

            entity.CopyPropertiesTo(existingEntity);

            await _unitOfWork.GetRepository<T>().Update(existingEntity).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public async Task<T?> Get(Guid id)
        {
            return await _unitOfWork.GetRepository<T>().GetById(id).ConfigureAwait(false);
        }

        public async Task<PagedList<T>> Get(GeneralQueryFilter filters, params Expression<Func<T, object>>[] includes)
        {
            var entities = _unitOfWork.GetRepository<T>().Get(null, includes);

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

            if (filters.PageNumber == 0) filters.PageNumber = _paginationOptions.DefaultPageNumber;
            if (filters.PageSize == 0) filters.PageSize = _paginationOptions.DefaultPageSize;

            var pagedEntities = await PagedList<T>.CreateAsync(entities, filters.PageNumber, filters.PageSize).ConfigureAwait(false);

            return pagedEntities;
        }

        public async Task Delete(Guid id)
        {
            await _unitOfWork.GetRepository<T>().Delete(id).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}

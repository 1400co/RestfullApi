using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IGenericService<T> where T : BaseEntity, new()
    {
        Task Delete(Guid id);
        Task<PagedList<T>> Get(GeneralQueryFilter filters, params Expression<Func<T, object>>[] includes);
        Task<T> Get(Guid id);
        Task Insert(T entity);
        Task<bool> Update(T entity);
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);
    }
}
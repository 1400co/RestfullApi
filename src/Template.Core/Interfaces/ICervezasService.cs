using Template.Core.CustomEntities;
using Template.Core.Entities;
using Template.Core.QueryFilters;
using System;
using System.Threading.Tasks;

namespace Template.Core.Interfaces
{
    public interface ICervezasService
    {
        Task<PagedList<Cervezas>> GetCervezas(CervezasQueryFilter filters);
        Task<Cervezas?> GetCerveza(Guid id);
        Task InsertCerveza(Cervezas cerveza);
        Task<bool> UpdateCerveza(Cervezas cerveza);
        Task DeleteCerveza(Guid id);
    }
}

using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
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

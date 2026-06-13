using Microsoft.Extensions.Options;
using Template.Core.CustomEntities;
using Template.Core.Entities;
using Template.Core.Exceptions;
using Template.Core.Interfaces;
using Template.Core.QueryFilters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Template.Core.Services
{
    public class CervezasService : GenericService<Cervezas>, ICervezasService
    {
        public CervezasService(IUnitOfWork unitOfWork, IOptions<PaginationOptions> paginationOptions)
            : base(unitOfWork, paginationOptions)
        {
        }

        public async Task InsertCerveza(Cervezas cerveza)
        {
            if (string.IsNullOrWhiteSpace(cerveza.Nombre))
                throw new BusinessException("El nombre de la cerveza es requerido");

            if (cerveza.Precio <= 0)
                throw new BusinessException("El precio debe ser mayor a cero");

            if (cerveza.GradosAlcohol < 0 || cerveza.GradosAlcohol > 100)
                throw new BusinessException("Los grados de alcohol deben estar entre 0 y 100");

            await _unitOfWork.GetRepository<Cervezas>().Insert(cerveza).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<bool> UpdateCerveza(Cervezas cerveza)
        {
            var existingCerveza = await _unitOfWork.GetRepository<Cervezas>().GetById(cerveza.Id).ConfigureAwait(false);
            if (existingCerveza == null)
                throw new BusinessException("La cerveza no existe");

            if (string.IsNullOrWhiteSpace(cerveza.Nombre))
                throw new BusinessException("El nombre de la cerveza es requerido");

            if (cerveza.Precio <= 0)
                throw new BusinessException("El precio debe ser mayor a cero");

            if (cerveza.GradosAlcohol < 0 || cerveza.GradosAlcohol > 100)
                throw new BusinessException("Los grados de alcohol deben estar entre 0 y 100");

            cerveza.CopyPropertiesTo(existingCerveza);

            await _unitOfWork.GetRepository<Cervezas>().Update(existingCerveza).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        public async Task<Cervezas?> GetCerveza(Guid id)
        {
            return await _unitOfWork.GetRepository<Cervezas>().GetById(id).ConfigureAwait(false);
        }

        public async Task<PagedList<Cervezas>> GetCervezas(CervezasQueryFilter filters)
        {
            var cervezas = _unitOfWork.GetRepository<Cervezas>().Get();

            if (filters.PageNumber == 0) filters.PageNumber = _paginationOptions.DefaultPageNumber;
            if (filters.PageSize == 0) filters.PageSize = _paginationOptions.DefaultPageSize;

            if (!string.IsNullOrWhiteSpace(filters.Nombre))
                cervezas = cervezas.Where(x => x.Nombre.ToLower().Contains(filters.Nombre.ToLower()));

            if (filters.GradosAlcoholMin.HasValue)
                cervezas = cervezas.Where(x => x.GradosAlcohol >= filters.GradosAlcoholMin.Value);

            if (filters.GradosAlcoholMax.HasValue)
                cervezas = cervezas.Where(x => x.GradosAlcohol <= filters.GradosAlcoholMax.Value);

            if (filters.PrecioMin.HasValue)
                cervezas = cervezas.Where(x => x.Precio >= filters.PrecioMin.Value);

            if (filters.PrecioMax.HasValue)
                cervezas = cervezas.Where(x => x.Precio <= filters.PrecioMax.Value);

            var pagedCervezas = await PagedList<Cervezas>.CreateAsync(cervezas, filters.PageNumber, filters.PageSize).ConfigureAwait(false);

            return pagedCervezas;
        }

        public async Task DeleteCerveza(Guid id)
        {
            await _unitOfWork.GetRepository<Cervezas>().Delete(id).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}

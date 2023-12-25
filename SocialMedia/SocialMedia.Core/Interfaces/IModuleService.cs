using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Interfaces
{
    public interface IModuleService
    {
        Task Delete(Guid id);
        Task<Modules> Get(Guid id);
        PagedList<Modules> Get(ModulesQueryFilter filters);
        Task Insert(Modules input);
        Task<bool> Update(Modules role); 
        IEnumerable<Modules> GetAll();
    }
}
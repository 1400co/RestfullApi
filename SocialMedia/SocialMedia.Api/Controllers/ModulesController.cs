using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Api.Responses;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Dtos;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[Controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService _moduleService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public ModulesController(IModuleService moduleService, IMapper mapper, IUriService uriService)
        {
            _moduleService = moduleService;
            _mapper = mapper;
            _uriService = uriService;
        }

        /// <summary>
        /// Obtiene todos los módulos disponibles.
        /// </summary>
        /// <returns>Un resultado HTTP que contiene una lista de módulos en forma de objetos `ModulesDto`.</returns>
        /// <remarks>
        /// Este método permite obtener todos los módulos disponibles en el sistema. Utiliza el servicio correspondiente (`_moduleService.GetAll()`) para recuperar la lista de módulos desde la base de datos.
        /// Luego, mapea los objetos de módulo a objetos `ModulesDto` utilizando AutoMapper y los coloca en una respuesta HTTP en forma de lista.
        /// Finalmente, devuelve una respuesta HTTP que contiene la lista de módulos en forma de objetos `ModulesDto`.
        /// </remarks>
        [HttpGet]
        [Route("getAll")]
        public IActionResult getAll()
        {
            var barriosList = _moduleService.GetAll();
            var barriosDto = _mapper.Map<IEnumerable<ModulesDto>>(barriosList);
            var response = new ApiResponse<IEnumerable<ModulesDto>>(barriosDto);

            return Ok(response);
        }


        /// <summary>
        /// Obtiene una lista paginada de módulos con opciones de filtro.
        /// </summary>
        /// <param name="filters">Los filtros que se aplicarán a la consulta de módulos.</param>
        /// <returns>Un resultado HTTP que contiene una lista paginada de módulos en forma de objetos `ModulesDto`.</returns>
        /// <remarks>
        /// Este método permite obtener una lista paginada de módulos aplicando filtros opcionales. Utiliza el servicio correspondiente (`_moduleService.Get(filters)`) para realizar la consulta en la base de datos.
        /// Luego, mapea los objetos de módulo a objetos `ModulesDto` utilizando AutoMapper y los coloca en una respuesta HTTP en forma de lista paginada.
        /// Además, incluye metadatos de paginación en la respuesta para que los clientes puedan navegar por las páginas de resultados.
        /// Finalmente, devuelve una respuesta HTTP que contiene la lista paginada de módulos en forma de objetos `ModulesDto`.
        /// </remarks>
        [HttpGet(Name = nameof(GetModules))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<ModulesDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetModules([FromQuery] ModulesQueryFilter filters)
        {
            var Modules = _moduleService.Get(filters);
            var ModulesDto = _mapper.Map<IEnumerable<ModulesDto>>(Modules);
            var response = new ApiResponse<IEnumerable<ModulesDto>>(ModulesDto);

            var metaData = new Metadata
            {
                TotalCount = Modules.TotalCount,
                PageSize = Modules.PageSize,
                CurrentPage = Modules.CurrentPage,
                TotalPages = Modules.TotalPages,
                HasNextPage = Modules.HasNextPage,
                HasPreviousPage = Modules.HasPreviousPage,
            };

            response.Meta = metaData;
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));

            return Ok(response);
        }

        /// <summary>
        /// Obtiene un módulo por su identificador único.
        /// </summary>
        /// <param name="id">El identificador único del módulo que se desea obtener.</param>
        /// <returns>Un resultado HTTP que contiene el módulo en forma de objeto `ModulesDto`.</returns>
        /// <remarks>
        /// Este método permite obtener un módulo específico de la base de datos utilizando su identificador único. Utiliza el servicio correspondiente (`_moduleService.Get(id)`) para realizar la consulta en la base de datos.
        /// Luego, mapea el objeto de módulo a un objeto `ModulesDto` utilizando AutoMapper y lo coloca en una respuesta HTTP.
        /// Finalmente, devuelve una respuesta HTTP que contiene el módulo en forma de objeto `ModulesDto`.
        /// </remarks>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var Module = await _moduleService.Get(id);
            var ModuleDto = _mapper.Map<ModulesDto>(Module);
            var response = new ApiResponse<ModulesDto>(ModuleDto);
            return Ok(response);
        }

        /// <summary>
        /// Crea un nuevo módulo.
        /// </summary>
        /// <param name="ModulesDto">Los datos del módulo que se va a crear.</param>
        /// <returns>Un resultado HTTP que indica que el módulo se ha creado con éxito.</returns>
        /// <remarks>
        /// Este método permite crear un nuevo módulo en la base de datos. Recibe los datos del módulo en forma de objeto `ModulesDto`, los mapea a un objeto `Modules` y luego realiza la inserción en la base de datos a través del servicio correspondiente (`_moduleService.Insert(Module)`).
        /// Luego, mapea el objeto de módulo creado de nuevo a un objeto `ModulesDto` utilizando AutoMapper y lo coloca en una respuesta HTTP.
        /// Finalmente, devuelve una respuesta HTTP que indica que el módulo se ha creado con éxito.
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> Post(ModulesDto ModulesDto)
        {
            var Module = _mapper.Map<Modules>(ModulesDto);

            await _moduleService.Insert(Module);

            ModulesDto = _mapper.Map<ModulesDto>(Module);

            var response = new ApiResponse<ModulesDto>(ModulesDto);
            return Ok(response);
        }

        /// <summary>
        /// Actualiza un módulo existente.
        /// </summary>
        /// <param name="id">El ID del módulo que se va a actualizar.</param>
        /// <param name="ModulesDto">Los nuevos datos del módulo.</param>
        /// <returns>Un resultado HTTP que indica que el módulo se ha actualizado con éxito.</returns>
        /// <remarks>
        /// Este método permite actualizar un módulo existente en la base de datos. Recibe el ID del módulo a actualizar y los nuevos datos del módulo en forma de objeto `ModulesDto`.
        /// Luego, mapea los datos del objeto `ModulesDto` a un objeto `Modules` y establece el ID proporcionado.
        /// Realiza la actualización en la base de datos a través del servicio correspondiente (`_moduleService.Update(Module)`).
        /// Finalmente, devuelve una respuesta HTTP que indica que el módulo se ha actualizado con éxito.
        /// </remarks>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, ModulesDto ModulesDto)
        {
            var Module = _mapper.Map<Modules>(ModulesDto);
            Module.Id = id;

            await _moduleService.Update(Module);

            return Ok();
        }

        /// <summary>
        /// Elimina un módulo existente.
        /// </summary>
        /// <param name="id">El ID del módulo que se va a eliminar.</param>
        /// <returns>Un resultado HTTP que indica que el módulo se ha eliminado con éxito.</returns>
        /// <remarks>
        /// Este método permite eliminar un módulo existente en la base de datos. Recibe el ID del módulo a eliminar como parámetro.
        /// Luego, utiliza el servicio correspondiente (`_moduleService.Delete(id)`) para llevar a cabo la eliminación en la base de datos.
        /// Finalmente, devuelve una respuesta HTTP que indica que el módulo se ha eliminado con éxito.
        /// </remarks>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _moduleService.Delete(id);
            return Ok();
        }
    }
}

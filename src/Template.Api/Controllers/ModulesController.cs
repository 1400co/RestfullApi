using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Template.Api.Responses;
using Template.Core.CustomEntities;
using Template.Core.Dtos;
using Template.Core.Entities;
using Template.Core.Interfaces;
using Template.Core.QueryFilters;
using Template.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Template.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
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

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var barriosList = _moduleService.GetAll();
            var barriosDto = _mapper.Map<IEnumerable<ModulesDto>>(barriosList);
            var response = new ApiResponse<IEnumerable<ModulesDto>>(barriosDto);
            response.Links = new List<LinkInfo>
            {
                new() { Rel = "self", Href = Request.Path.Value!, Method = "GET" },
                new() { Rel = "create", Href = Request.Path.Value!.Replace("/all", ""), Method = "POST" },
            };
            return Ok(response);
        }

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
                NextPageUrl = Modules.HasNextPage
                    ? _uriService.GetPageUri(Modules.CurrentPage + 1, Modules.PageSize, filters.Filter, Request.Path.Value!).ToString()
                    : null,
                PreviousPageUrl = Modules.HasPreviousPage
                    ? _uriService.GetPageUri(Modules.CurrentPage - 1, Modules.PageSize, filters.Filter, Request.Path.Value!).ToString()
                    : null,
                Links = new List<LinkInfo>
                {
                    new() { Rel = "self", Href = Request.Path.Value!, Method = "GET" },
                    new() { Rel = "create", Href = Request.Path.Value!, Method = "POST" },
                    new() { Rel = "all", Href = $"{Request.Path.Value}/all", Method = "GET" },
                }
            };

            response.Meta = metaData;
            response.Links = metaData.Links;
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));

            return Ok(response);
        }

        [HttpGet("{id}", Name = nameof(GetModuleById))]
        public async Task<IActionResult> GetModuleById(Guid id)
        {
            var Module = await _moduleService.Get(id);
            if (Module == null) return NotFound();
            var ModuleDto = _mapper.Map<ModulesDto>(Module);
            var response = new ApiResponse<ModulesDto>(ModuleDto);
            response.Links = new List<LinkInfo>
            {
                new() { Rel = "self", Href = $"{Request.Path.Value}", Method = "GET" },
                new() { Rel = "update", Href = $"{Request.Path.Value}", Method = "PUT" },
                new() { Rel = "delete", Href = $"{Request.Path.Value}", Method = "DELETE" },
                new() { Rel = "collection", Href = Request.Path.Value![..Request.Path.Value!.LastIndexOf('/')], Method = "GET" },
            };
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ModulesDto ModulesDto)
        {
            var Module = _mapper.Map<Modules>(ModulesDto);

            await _moduleService.Insert(Module);

            ModulesDto = _mapper.Map<ModulesDto>(Module);

            var response = new ApiResponse<ModulesDto>(ModulesDto);
            return CreatedAtAction(nameof(GetModuleById), new { id = Module.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, ModulesDto ModulesDto)
        {
            var Module = _mapper.Map<Modules>(ModulesDto);
            Module.Id = id;

            await _moduleService.Update(Module);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _moduleService.Delete(id);
            return NoContent();
        }
    }
}

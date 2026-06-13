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
    public class RolModuleController : ControllerBase
    {
        private readonly IRolModuleService _rolModuleService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public RolModuleController(IRolModuleService rolModuleService, IMapper mapper, IUriService uriService)
        {
            _rolModuleService = rolModuleService;
            _mapper = mapper;
            _uriService = uriService;
        }

        [HttpGet(Name = nameof(GetRolModules))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<RolModuleDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRolModules([FromQuery] RolModuleQueryFilter filters)
        {
            var rolModules = await _rolModuleService.GetRolModules(filters);
            var rolModuleDto = _mapper.Map<IEnumerable<RolModuleDto>>(rolModules);
            var response = new ApiResponse<IEnumerable<RolModuleDto>>(rolModuleDto);

            var metaData = new Metadata
            {
                TotalCount = rolModules.TotalCount,
                PageSize = rolModules.PageSize,
                CurrentPage = rolModules.CurrentPage,
                TotalPages = rolModules.TotalPages,
                HasNextPage = rolModules.HasNextPage,
                HasPreviousPage = rolModules.HasPreviousPage,
                NextPageUrl = rolModules.HasNextPage
                    ? _uriService.GetPageUri(rolModules.CurrentPage + 1, rolModules.PageSize, filters.Filter, Request.Path.Value!).ToString()
                    : null,
                PreviousPageUrl = rolModules.HasPreviousPage
                    ? _uriService.GetPageUri(rolModules.CurrentPage - 1, rolModules.PageSize, filters.Filter, Request.Path.Value!).ToString()
                    : null,
                Links = new List<LinkInfo>
                {
                    new() { Rel = "self", Href = Request.Path.Value!, Method = "GET" },
                    new() { Rel = "create", Href = Request.Path.Value!, Method = "POST" },
                }
            };

            response.Meta = metaData;
            response.Links = metaData.Links;
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));

            return Ok(response);
        }

        [HttpGet("{id}", Name = nameof(GetRolModuleById))]
        public async Task<IActionResult> GetRolModuleById(Guid id)
        {
            var rolModule = await _rolModuleService.GetRolModule(id);
            if (rolModule == null) return NotFound();
            var rolModuleDto = _mapper.Map<RolModuleDto>(rolModule);
            var response = new ApiResponse<RolModuleDto>(rolModuleDto);
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
        public async Task<IActionResult> Post(RolModuleDto rolModuleDto)
        {
            var rolModule = _mapper.Map<RolModule>(rolModuleDto);

            await _rolModuleService.InsertRolModule(rolModule);

            rolModuleDto = _mapper.Map<RolModuleDto>(rolModule);

            var response = new ApiResponse<RolModuleDto>(rolModuleDto);
            return CreatedAtAction(nameof(GetRolModuleById), new { id = rolModule.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, RolModuleDto rolModuleDto)
        {
            var rolModule = _mapper.Map<RolModule>(rolModuleDto);
            rolModule.Id = id;

            await _rolModuleService.UpdateRolModule(rolModule);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _rolModuleService.DeleteRolModule(id);
            return NoContent();
        }
    }
}

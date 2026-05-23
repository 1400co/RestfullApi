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
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RolModuleController(IRolModuleService rolModuleService, IMapper mapper) : ControllerBase
    {

        [HttpGet(Name = nameof(GetRolModules))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<RolModuleDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRolModules([FromQuery] RolModuleQueryFilter filters)
        {
            var rolModules = await rolModuleService.GetRolModules(filters);
            var rolModuleDto = mapper.Map<IEnumerable<RolModuleDto>>(rolModules);
            var response = new ApiResponse<IEnumerable<RolModuleDto>>(rolModuleDto);

            var metaData = new Metadata
            {
                TotalCount = rolModules.TotalCount,
                PageSize = rolModules.PageSize,
                CurrentPage = rolModules.CurrentPage,
                TotalPages = rolModules.TotalPages,
                HasNextPage = rolModules.HasNextPage,
                HasPreviousPage = rolModules.HasPreviousPage,
            };

            response.Meta = metaData;
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var rolModule = await rolModuleService.GetRolModule(id);
            if (rolModule == null) return NotFound();
            var rolModuleDto = mapper.Map<RolModuleDto>(rolModule);
            var response = new ApiResponse<RolModuleDto>(rolModuleDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RolModuleDto rolModuleDto)
        {
            var rolModule = mapper.Map<RolModule>(rolModuleDto);

            await rolModuleService.InsertRolModule(rolModule);

            rolModuleDto = mapper.Map<RolModuleDto>(rolModule);

            var response = new ApiResponse<RolModuleDto>(rolModuleDto);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, RolModuleDto rolModuleDto)
        {
            var rolModule = mapper.Map<RolModule>(rolModuleDto);
            rolModule.Id = id;

            await rolModuleService.UpdateRolModule(rolModule);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await rolModuleService.DeleteRolModule(id);
            return Ok();
        }
    }
}

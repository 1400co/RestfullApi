﻿using AutoMapper;
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
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public RolesController(IRolesService rolesService, IMapper mapper, IUriService uriService)
        {
            _rolesService = rolesService;
            _mapper = mapper;
            _uriService = uriService;
        }

        /// <summary>
        /// Obtiene todos los roles disponibles.
        /// </summary>
        /// <returns>Un resultado HTTP que contiene una lista de roles en forma de objetos RolesDto.</returns>
        /// <remarks>
        /// Este método se utiliza para obtener todos los roles disponibles en el sistema. 
        /// Utiliza el servicio correspondiente (`_rolesService.GetAll()`) para recuperar la lista de roles desde la base de datos.
        /// Luego, mapea estos roles a objetos RolesDto y los devuelve en una respuesta HTTP.
        /// </remarks>
        [HttpGet]
        [Route("getAll")]
        public IActionResult getAll()
        {
            var barriosList = _rolesService.GetAll();
            var barriosDto = _mapper.Map<IEnumerable<RolesDto>>(barriosList);
            var response = new ApiResponse<IEnumerable<RolesDto>>(barriosDto);

            return Ok(response);
        }

        [HttpGet(Name = nameof(GetRoles))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<RolesDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetRoles([FromQuery] RolesQueryFilter filters)
        {
            var roles = await _rolesService.GetRoles(filters);
            var rolesDto = _mapper.Map<IEnumerable<RolesDto>>(roles);
            var response = new ApiResponse<IEnumerable<RolesDto>>(rolesDto);

            var metaData = new Metadata
            {
                TotalCount = roles.TotalCount,
                PageSize = roles.PageSize,
                CurrentPage = roles.CurrentPage,
                TotalPages = roles.TotalPages,
                HasNextPage = roles.HasNextPage,
                HasPreviousPage = roles.HasPreviousPage,
            };

            response.Meta = metaData;
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var role = await _rolesService.GetRole(id);
            var roleDto = _mapper.Map<RolesDto>(role);
            var response = new ApiResponse<RolesDto>(roleDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RolesDto rolesDto)
        {
            var role = _mapper.Map<Roles>(rolesDto);

            await _rolesService.InsertRole(role);

            rolesDto = _mapper.Map<RolesDto>(role);

            var response = new ApiResponse<RolesDto>(rolesDto);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, RolesDto rolesDto)
        {
            var role = _mapper.Map<Roles>(rolesDto);
            role.Id = id;

            await _rolesService.UpdateRole(role);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _rolesService.DeleteRole(id);
            return Ok();
        }
    }
}

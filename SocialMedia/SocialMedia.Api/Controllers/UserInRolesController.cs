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
    [ApiController]
    [Route("api/[controller]")]
    public class UserInRolesController : ControllerBase
    {
        private readonly IUserInRolesService _userInRolesService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;

        public UserInRolesController(IUserInRolesService userInRolesService, IMapper mapper, IUriService uriService)
        {
            _userInRolesService = userInRolesService;
            _mapper = mapper;
            _uriService = uriService;
        }

        [HttpGet(Name = nameof(GetUserInRoles))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UserInRolesDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserInRoles([FromQuery] UserInRolesQueryFilter filters)
        {
            var userInRoles = await _userInRolesService.GetUserInRoles(filters);
            var userInRolesDto = _mapper.Map<IEnumerable<UserInRolesDto>>(userInRoles);
            var response = new ApiResponse<IEnumerable<UserInRolesDto>>(userInRolesDto);

            var metaData = new Metadata
            {
                TotalCount = userInRoles.TotalCount,
                PageSize = userInRoles.PageSize,
                CurrentPage = userInRoles.CurrentPage,
                TotalPages = userInRoles.TotalPages,
                HasNextPage = userInRoles.HasNextPage,
                HasPreviousPage = userInRoles.HasPreviousPage,
            };

            response.Meta = metaData;
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var userInRole = await _userInRolesService.GetUserInRole(id);
            var userInRoleDto = _mapper.Map<UserInRolesDto>(userInRole);
            var response = new ApiResponse<UserInRolesDto>(userInRoleDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserInRolesDto userInRolesDto)
        {
            var userInRole = _mapper.Map<UserInRoles>(userInRolesDto);

            await _userInRolesService.InsertUserInRole(userInRole);

            userInRolesDto = _mapper.Map<UserInRolesDto>(userInRole);

            var response = new ApiResponse<UserInRolesDto>(userInRolesDto);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UserInRolesDto userInRolesDto)
        {
            var userInRole = _mapper.Map<UserInRoles>(userInRolesDto);
            userInRole.Id = id;

            await _userInRolesService.UpdateUserInRole(userInRole);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userInRolesService.DeleteUserInRole(id);
            return Ok();
        }
    }

}

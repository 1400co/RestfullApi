using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Template.Api.Responses;
using Template.Core.CustomEntities;
using Template.Core.Dtos;
using Template.Core.Entities;
using Template.Core.Enumerations;
using Template.Core.Exceptions;
using Template.Core.Interfaces;
using Template.Core.QueryFilters;
using Template.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Template.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ISecurityService _securityService;
        private readonly IEmailService _emailService;
        private readonly IUriService _uriService;

        public UserController(IUserService userService, IMapper mapper, ISecurityService securityService,
            IEmailService emailService, IUriService uriService)
        {
            _userService = userService;
            _mapper = mapper;
            _securityService = securityService;
            _emailService = emailService;
            _uriService = uriService;
        }

        [HttpGet(Name = nameof(GetUsers))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UserDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUsers([FromQuery] UserQueryFilter filters)
        {
            var users = await _userService.GetUsers(filters);
            var userDto = _mapper.Map<IEnumerable<UserDto>>(users);

            userDto.ToList().ForEach(user =>
            {
                var dbUser = users.FirstOrDefault(u => u.Id == user.Id);
                user.Roles = dbUser != null
                    ? string.Join(", ", dbUser.Roles.Select(r => r.ToString()))
                    : string.Empty;
            });

            var response = new ApiResponse<IEnumerable<UserDto>>(userDto);

            var metaData = new Metadata
            {
                TotalCount = users.TotalCount,
                PageSize = users.PageSize,
                CurrentPage = users.CurrentPage,
                TotalPages = users.TotalPages,
                HasNextPage = users.HasNextPage,
                HasPreviousPage = users.HasPreviousPage,
                NextPageUrl = users.HasNextPage
                    ? _uriService.GetPageUri(users.CurrentPage + 1, users.PageSize, filters.Filter, Request.Path.Value!).ToString()
                    : null,
                PreviousPageUrl = users.HasPreviousPage
                    ? _uriService.GetPageUri(users.CurrentPage - 1, users.PageSize, filters.Filter, Request.Path.Value!).ToString()
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

        [HttpGet("{id}", Name = nameof(GetUserById))]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetUser(id);
            if (user == null) return NotFound();
            var userDto = _mapper.Map<UserDto>(user);
            var response = new ApiResponse<UserDto>(userDto);
            response.Links = new List<LinkInfo>
            {
                new() { Rel = "self", Href = $"{Request.Path.Value}", Method = "GET" },
                new() { Rel = "update", Href = $"{Request.Path.Value}", Method = "PUT" },
                new() { Rel = "delete", Href = $"{Request.Path.Value}", Method = "DELETE" },
                new() { Rel = "collection", Href = Request.Path.Value![..Request.Path.Value!.LastIndexOf('/')], Method = "GET" },
            };
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(UserDto userDto)
        {
            var sec = await _securityService.GetUserByEmail(userDto.Email);

            if (sec != null)
            {
                throw new BusinessException("Usuario ya existe.");
            }

            var user = _mapper.Map<User>(userDto);
            user.Id = Guid.NewGuid();
            user.Roles = new List<RoleType> { RoleType.Consumer };

            await _userService.InsertUser(user);

            BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(
                userDto.Email,
                "Bienvenido",
                $"¡Bienvenido a nuestra plataforma!\n\nPara empezar a utilizar la plataforma, haz clic en el siguiente enlace:\n\n<a href='URL_DE_LA_PLATAFORMA'>Ingresar a la plataforma</a>",
                true
            ));

            userDto = _mapper.Map<UserDto>(user);

            var response = new ApiResponse<UserDto>(userDto);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Id = id;

            await _userService.UpdateUser(user);

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, [FromBody] JsonPatchDocument<UserDto> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var user = await _userService.GetUser(id);
            if (user == null)
                return NotFound();

            var userDto = _mapper.Map<UserDto>(user);
            patchDoc.ApplyTo(userDto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _mapper.Map(userDto, user);
            await _userService.UpdateUser(user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteUser(id);
            return NoContent();
        }
    }
}

using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Api.Responses;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Dtos;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Enumerations;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SocialMedia.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService, IMapper mapper, ISecurityService securityService,
        IEmailService emailService) : ControllerBase
    {

        [HttpGet(Name = nameof(GetUsers))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UserDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUsers([FromQuery] UserQueryFilter filters)
        {
            var users = await userService.GetUsers(filters);
            var userDto = mapper.Map<IEnumerable<UserDto>>(users);

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
            };

            response.Meta = metaData;
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await userService.GetUser(id);
            var userDto = mapper.Map<UserDto>(user);
            var response = new ApiResponse<UserDto>(userDto);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(UserDto userDto)
        {
            var sec = await securityService.GetUserByEmail(userDto.Email);

            if (sec != null )
            {
                throw new BusinessException("Usuario ya existe.");
            }

            var user = mapper.Map<User>(userDto);
            user.Id = Guid.NewGuid();
            user.Roles = new List<RoleType> { RoleType.Consumer };

            await userService.InsertUser(user);

            BackgroundJob.Enqueue(() => emailService.SendEmailAsync(
                userDto.Email,
                "Bienvenido",
                $"¡Bienvenido a nuestra plataforma!\n\nPara empezar a utilizar la plataforma, haz clic en el siguiente enlace:\n\n<a href='URL_DE_LA_PLATAFORMA'>Ingresar a la plataforma</a>",
                true
            ));

            userDto = mapper.Map<UserDto>(user);

            var response = new ApiResponse<UserDto>(userDto);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UserDto userDto)
        {
            var user = mapper.Map<User>(userDto);
            user.Id = id;

            await userService.UpdateUser(user);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await userService.DeleteUser(id);
            return Ok();
        }
    }
}

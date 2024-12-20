﻿using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SocialMedia.Api.Responses;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Dtos;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.QueryFilters;
using SocialMedia.Infrastructure.Interfaces;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly ISecurityService _securityService;
        private readonly IPasswordService _passwordService;
        private readonly IUserInRolesService _userInRolesService;
        private readonly IEmailService _emailService;
        private readonly Guid _administratorRole = Guid.Parse("7C2E1E9B-410B-4A6B-B9AE-8B078422EB2D");
        private readonly Guid _usuarioRole = Guid.Parse("7c2e1e9b-410b-4a6b-b9ae-8b078422eb2c");

        public UserController(IUserService userService, IMapper mapper, IUriService uriService, ISecurityService securityService, 
        IPasswordService passwordService, IUserInRolesService userInRolesService, IEmailService emailService)
        {
            _userService = userService;
            _mapper = mapper;
            _uriService = uriService;
            _securityService = securityService;
            _passwordService = passwordService;
            _userInRolesService = userInRolesService;
            _emailService = emailService;
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
                var roles = this._userInRolesService.GetAll(user.Id)
                    .Select(role => role.Roles.RolName) 
                    .ToList();

                // Usamos string.Join para concatenar los nombres de los roles separados por comas
                user.Roles = string.Join(", ", roles);
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
            var user = await _userService.GetUser(id);
            var userDto = _mapper.Map<UserDto>(user);
            var response = new ApiResponse<UserDto>(userDto);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(UserDto userDto)
        {
            var sec = await _securityService.GetUserByEmail(userDto.Email);

            if (sec != null )
            {
                throw new BusinessException("Usuario ya existe.");
            }

            var user = _mapper.Map<User>(userDto);
            user.Id = Guid.NewGuid();
            user.Role = Core.Enumerations.RoleType.Consumer;

            user.UserInRoles.Add(
                new UserInRoles()
                {
                    RoleId = _usuarioRole,
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    Responsable = "System"
                });

            await _userService.InsertUser(user);

            BackgroundJob.Enqueue(() => this._emailService.SendEmailAsync(
                userDto.Email,
                "Bienvenido",
                $"¡Bienvenido a nuestra plataforma!\n\nPara empezar a utilizar la plataforma, haz clic en el siguiente enlace:\n\n<a href='URL_DE_LA_PLATAFORMA'>Ingresar a la plataforma</a>",
                true
            ));

            userDto = _mapper.Map<UserDto>(user);

            var response = new ApiResponse<UserDto>(userDto);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Id = id;

            await _userService.UpdateUser(user);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userService.DeleteUser(id);
            return Ok();
        }
    }
}

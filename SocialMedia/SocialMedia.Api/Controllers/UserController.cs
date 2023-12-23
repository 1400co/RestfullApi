using AutoMapper;
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
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly ISecurityService _securityService;
        private readonly IPasswordService _passwordService;

        public UserController(IUserService userService, IMapper mapper, IUriService uriService, ISecurityService securityService, IPasswordService passwordService)
        {
            _userService = userService;
            _mapper = mapper;
            _uriService = uriService;
            _securityService = securityService;
            _passwordService = passwordService;
        }

        [HttpGet(Name = nameof(GetUsers))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UserDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task< IActionResult> GetUsers([FromQuery] UserQueryFilter filters)
        {
            var users = await  _userService.GetUsers(filters);
            var userDto = _mapper.Map<IEnumerable<UserDto>>(users);
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

        [HttpPost]
        public async Task<IActionResult> Post(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);

            await _userService.InsertUser(user);

            var credential = new Security()
            {
                UserId = user.Id,
                Role = Core.Enumerations.RoleType.Consumer,
                UserName = userDto.UserName,
                Password = _passwordService.Hash(userDto.Password),
                RefreshTokenExpiryTime = DateTime.Now.AddHours(1),
            };

            await _securityService.RegisterUser(credential);

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

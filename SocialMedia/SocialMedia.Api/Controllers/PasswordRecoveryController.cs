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
    public class PasswordRecoveryController : ControllerBase
    {
        private readonly IPasswordRecoveryService _passwordRecoveryService;
        private readonly ISecurityService _securityService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        private readonly IPasswordService passwordService;

        public PasswordRecoveryController(IPasswordRecoveryService passwordRecoveryService, IMapper mapper, IUriService uriService, IEmailService emailService, ISecurityService securityService, IUserService userService, IPasswordService passwordService)
        {
            _passwordRecoveryService = passwordRecoveryService;
            _mapper = mapper;
            _uriService = uriService;
            _emailService = emailService;
            _securityService = securityService;
            _userService = userService;
            this.passwordService = passwordService;
        }

        [HttpGet(Name = nameof(GetpasswordRecoverys))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<PasswordRecoveryDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetpasswordRecoverys([FromQuery] GeneralQueryFilter filters)
        {
            var passwordRecoverys = await _passwordRecoveryService.GetRecovery(filters);
            var passwordRecoveryDto = _mapper.Map<IEnumerable<PasswordRecoveryDto>>(passwordRecoverys);
            var response = new ApiResponse<IEnumerable<PasswordRecoveryDto>>(passwordRecoveryDto);

            var metaData = new Metadata
            {
                TotalCount = passwordRecoverys.TotalCount,
                PageSize = passwordRecoverys.PageSize,
                CurrentPage = passwordRecoverys.CurrentPage,
                TotalPages = passwordRecoverys.TotalPages,
                HasNextPage = passwordRecoverys.HasNextPage,
                HasPreviousPage = passwordRecoverys.HasPreviousPage,
            };

            response.Meta = metaData;
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metaData));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var passwordRecovery = await _passwordRecoveryService.GetRecovery(id);
            var passwordRecoveryDto = _mapper.Map<PasswordRecoveryDto>(passwordRecovery);
            var response = new ApiResponse<PasswordRecoveryDto>(passwordRecoveryDto);
            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post(PasswordRecoveryDto passwordRecoveryDto)
        {
            var security = await this._securityService.GetCredentialsByUserName(passwordRecoveryDto.UserName);

            if (security == null)
            {
                throw new BusinessException("User Does not exists");
            }

            passwordRecoveryDto.UserId = security.UserId;
            passwordRecoveryDto.ExpiryDate = DateTime.Now.AddMinutes(30);

            var passwordRecovery = _mapper.Map<PasswordRecovery>(passwordRecoveryDto);

            var recovery = await _passwordRecoveryService.InsertRecovery(passwordRecovery);

            BackgroundJob.Enqueue(() => this._emailService.SendEmailAsync("", "Passord recovery", $"password recovery {recovery.Id}", false));

            passwordRecoveryDto.Id = recovery.Id;

            var response = new ApiResponse<PasswordRecoveryDto>(passwordRecoveryDto);
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, PasswordRecoveryDto passwordRecoveryDto)
        {
            var passwordRecovery = _mapper.Map<PasswordRecovery>(passwordRecoveryDto);
            passwordRecovery.Id = id;

            await _passwordRecoveryService.UpdateRecovery(passwordRecovery);

            return Ok();
        }

        [HttpPut()]
        [AllowAnonymous]
        [Route("PasswordUpdate")]
        public async Task<IActionResult> PasswordUpdate(PasswordUpdate passwordUpdate)
        {
            var renewToken = await this._passwordRecoveryService.GetRecovery(passwordUpdate.Id);

            if (renewToken == null)
            {
                throw new BusinessException("Token expired.");
            }

            if (renewToken.ExpiryDate < DateTime.Now)
            {
                throw new BusinessException("Token expired.");
            }

            var user = await this._userService.GetUser(renewToken.UserId);

            if (user == null) { throw new BusinessException("User Dont exists."); }

            await this._securityService.UpdateRecoveryPassword(user.Id, passwordService.Hash(passwordUpdate.Password));

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _passwordRecoveryService.DeleteRecovery(id);
            return Ok();
        }


    }
}

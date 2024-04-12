using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SocialMedia.Core.Dtos;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TransforSerPu.Core.Dtos;
using TransforSerPu.Core.Interfaces;

namespace SocialMedia.Api.Controllers
{

    [AllowAnonymous]
    [Route("api/[Controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISecurityService _securityService;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IRolModuleService _rolModuleService;
        private readonly IUserInRolesService _userInRole;
        private readonly IPasswordRecoveryService _passwordRecoveryService;

        private string issuer;
        private string audience;
        private string secret;

        public TokenController(IConfiguration configuration, ISecurityService securityService, IPasswordService passwordService, ITokenService tokenService, IRolModuleService rolModuleService, IUserInRolesService userInRole, IPasswordRecoveryService passwordRecoveryService)
        {
            _configuration = configuration;
            _securityService = securityService;
            _passwordService = passwordService;
            _tokenService = tokenService;

            issuer = _configuration["Authentication:Issuer"];
            audience = _configuration["Authentication:Audience"];
            secret = _configuration["Authentication:SecretKey"];
            _rolModuleService = rolModuleService;
            _userInRole = userInRole;
            _passwordRecoveryService = passwordRecoveryService;
        }

        [HttpPost]
        public async Task<IActionResult> GetToken(UserLoginDto login)
        {
            try
            {
                if (login is null)
                {
                    return BadRequest("Invalid client request");
                }

                //If valid user
                var result = await ValidateUser(login);

                if (!result.Item1)
                {
                    return Unauthorized();
                }

                var security = result.Item2;

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, security.UserName),
                    new Claim(ClaimTypes.Role, security.Role.ToString()),
            };
                var accessToken = _tokenService.GenerateAccessToken(claims, issuer, audience, secret);
                var refreshToken = _tokenService.GenerateRefreshToken();
                var permisos = _rolModuleService.ObtenerModulosUsuario(security.UserId);

                await _securityService.UpdateRefreshToken(security.UserName, refreshToken);

                return Ok(new AuthenticatedResponse
                {
                    AuthToken = accessToken,
                    RefreshToken = refreshToken,
                    UserId = security.Id,
                    UserName = security.UserName,
                    Permisos = permisos
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenDto tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid client request");

            var result = await this._tokenService.RenewToken(tokenApiModel);

            return Ok(result);
        }

        [HttpPost]
        [Route("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var username = User.Identity.Name;
            var user = await _securityService.GetLoginByCredentials(new UserLoginDto() { User = username });
            if (user == null) return BadRequest();

            await _securityService.UpdateRefreshToken(username, null);

            return NoContent();
        }

        [HttpGet]
        [Route("me")]
        public async Task<IActionResult> Me()
        {
            var user = await this.GetCallerUser();
            if (user == null) return BadRequest();

            return Ok(user);
        }

        private async Task<(bool, Security)> ValidateUser(UserLoginDto login)
        {
            var user = await _securityService.GetLoginByCredentials(login);


            var isValid = _passwordService.Check(user.Password, login.Password);
            return (isValid, user);
        }

        private async Task<UserModelDto> GetCallerUser()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return null;
                }

                var username = User.Identity.Name;
                var user = await _securityService.GetLoginByCredentials(new UserLoginDto() { User = username });
                var roles = await _userInRole.GetUsersRoles(user.User.Id);

                return new UserModelDto()
                {
                    FullName = user.User.FullName,
                    Email = user.User.Email,    
                    UserName = username,
                    Roles = roles.Select(x=> x.Roles.RolName).ToList()
                };
            }
            catch (System.Exception e)
            {
                return null;
            }
        }

    }
}

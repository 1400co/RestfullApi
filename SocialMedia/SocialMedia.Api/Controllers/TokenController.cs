using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TransforSerPu.Core.Dtos;
using TransforSerPu.Core.Interfaces;

namespace SocialMedia.Api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISecurityService _securityService;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;

        private string issuer;
        private string audience;
        private string secret;

        public TokenController(IConfiguration configuration, ISecurityService securityService, IPasswordService passwordService, ITokenService tokenService)
        {
            _configuration = configuration;
            _securityService = securityService;
            _passwordService = passwordService;
            _tokenService = tokenService;

            issuer = _configuration["Authentication:Issuer"];
            audience = _configuration["Authentication:Audience"];
            secret = _configuration["Authentication:SecretKey"];
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

                await _securityService.UpdateRefreshToken(security.UserName, refreshToken);

                return Ok(new AuthenticatedResponse
                {
                    AuthToken = accessToken,
                    RefreshToken = refreshToken,
                    UserId = security.Id,
                    UserName = security.UserName,
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

        private async Task<(bool, Security)> ValidateUser(UserLoginDto login)
        {
            var user = await _securityService.GetLoginByCredentials(login);

            var isValid = _passwordService.Check(user.Password, login.Password);
            return (isValid, user);
        }

        private string GenerateToken(Security security)
        {
            //Header
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);

            //Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, security.UserName),
                new Claim("User", security.UserName),
                new Claim(ClaimTypes.Role, security.Role.ToString()),
            };

            //Payload
            var payload = new JwtPayload
            (
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.Now.AddMinutes(1)
            );

            var token = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

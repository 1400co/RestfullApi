using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SocialMedia.Core.Dtos;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Infrastructure.Interfaces;
using SocialMedia.Infrastructure.Services;
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
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        private string issuer;
        private string audience;
        private string secret;

        public TokenController(IConfiguration configuration, ISecurityService securityService, IPasswordService passwordService,
            ITokenService tokenService, IRolModuleService rolModuleService, IUserInRolesService userInRole, IUserService userService, IEmailService emailService)
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
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("RequestToken")]
        public async Task<IActionResult> RequestToken(string email)
        {
            var user = await this._userService.GetUserByEmail(email);

            if (user == null) { return NotFound(); }

            var otp = await _securityService.GetOneTimePassword(user.Id);

            // Estructura HTML para el cuerpo del mensaje
            var emailBody = $@"
                <html>
                    <body style='font-family: Arial, sans-serif; color: #333;'>
                        <h2 style='color: #0056b3;'>¡Bienvenido a nuestra plataforma!</h2>
                        <p>Estamos encantados de que te unas a nosotros.</p>
                        <p>Tu clave de un solo uso (OTP) es:</p>
                        <h3 style='color: #ff6600;'>{otp.Password}</h3>
                        <p>Para empezar a utilizar la plataforma, haz clic en el siguiente enlace:</p>
                        <p>Si no solicitaste este código, por favor ignora este mensaje.</p>
                        <br>
                        <p>Atentamente,<br>El equipo de soporte</p>
                    </body>
                </html>";

            // Envío del correo electrónico usando HTML
            BackgroundJob.Enqueue(() => this._emailService.SendEmailAsync(
                user.Email,
                "Bienvenido",
                emailBody,
                true // Activa el modo HTML para el correo electrónico
            ));

            return Ok();
        }



        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(Login login)
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

                var user = result.Item2;

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
            };
                var accessToken = _tokenService.GenerateAccessToken(claims, issuer, audience, secret);
                var refreshToken = _tokenService.GenerateRefreshToken();
                var permisos = _rolModuleService.ObtenerModulosUsuario(user.Id);


                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2);

                await this._userService.UpdateUser(user);

                return Ok(new AuthenticatedResponse
                {
                    AuthToken = accessToken,
                    RefreshToken = refreshToken,
                    UserId = user.Id,
                    UserName = user.Email,
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
            var user = await _userService.GetUserByEmail(username);
            if (user == null) return BadRequest();

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.UtcNow;

            await _userService.UpdateUser(user);

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

        private async Task<(bool, User)> ValidateUser(Login login)
        {
            var user = await this._userService.GetUserByEmail(login.Email);

            var result = await _securityService.ValidateCredentials(login.Email, login.Otp);

            return (result, user);
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
                var user = await _userService.GetUserByEmail(username);
                var roles = await _userInRole.GetUsersRoles(user.Id);

                return new UserModelDto()
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    UserName = username,
                    Roles = roles.Select(x => x.Roles.RolName).ToList()
                };
            }
            catch (System.Exception e)
            {
                return null;
            }
        }

    }
}

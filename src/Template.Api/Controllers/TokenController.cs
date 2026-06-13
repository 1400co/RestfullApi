using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Template.Core.Dtos;
using Template.Core.Entities;
using Template.Core.Interfaces;
using Template.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Template.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ISecurityService _securityService;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IRolModuleService _rolModuleService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly ISessionService _sessionService;

        private readonly string issuer;
        private readonly string audience;
        private readonly string secret;

        public TokenController(IConfiguration configuration, ISecurityService securityService, IPasswordService passwordService,
            ITokenService tokenService, IRolModuleService rolModuleService, IUserService userService, IEmailService emailService,
            ISessionService sessionService)
        {
            _configuration = configuration;
            _securityService = securityService;
            _passwordService = passwordService;
            _tokenService = tokenService;

            issuer = _configuration["Authentication:Issuer"]!;
            audience = _configuration["Authentication:Audience"]!;
            secret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
                ?? _configuration["Authentication:SecretKey"]!;
            _rolModuleService = rolModuleService;
            _userService = userService;
            _emailService = emailService;
            _sessionService = sessionService;
        }

        [AllowAnonymous]
        [EnableRateLimiting("AuthPolicy")]
        [HttpPost("otp")]
        public async Task<IActionResult> RequestOtp(string email)
        {
            var user = await _userService.GetUserByEmail(email);

            if (user == null) { return NotFound(); }

            var otp = await _securityService.GetOneTimePassword(user.Id);

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

            BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(
                user.Email,
                "Bienvenido",
                emailBody,
                true
            ));

            return NoContent();
        }

        [AllowAnonymous]
        [EnableRateLimiting("AuthPolicy")]
        [HttpPost("login")]
        public async Task<IActionResult> Login(Login login)
        {
            try
            {
                if (login is null)
                {
                    return BadRequest("Invalid client request");
                }

                var result = await ValidateUser(login);

                if (!result.Item1)
                {
                    return Unauthorized();
                }

                var user = result.Item2!;

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                };
                user.Roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role.ToString())));
                var accessToken = _tokenService.GenerateAccessToken(claims, issuer, audience, secret);
                var refreshToken = _tokenService.GenerateRefreshToken();
                var permisos = await _rolModuleService.ObtenerModulosUsuario(user.Id);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2);

                await _userService.UpdateUser(user);

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

        [HttpPost("renew")]
        public async Task<IActionResult> RenewToken(TokenDto tokenApiModel)
        {
            if (tokenApiModel is null)
                return BadRequest("Invalid client request");

            var result = await _tokenService.RenewToken(tokenApiModel);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Revoke()
        {
            var username = _sessionService.GetUserName()!;
            var user = await _userService.GetUserByEmail(username);
            if (user == null) return BadRequest();

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.UtcNow;

            await _userService.UpdateUser(user);

            return NoContent();
        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            var user = _sessionService.GetCurrentUser();
            if (user == null) return BadRequest();

            return Ok(user);
        }

        private async Task<(bool, User?)> ValidateUser(Login login)
        {
            var user = await _userService.GetUserByEmail(login.Email);

            var result = await _securityService.ValidateCredentials(login.Email, login.Otp);

            return (result, user);
        }
    }
}

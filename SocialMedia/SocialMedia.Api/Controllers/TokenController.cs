using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMedia.Api.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult GetToken(UserLogin login)
        {
            //If valid user
            if (ValidateUser(login))
            {
                return Ok(new { Token = GenerateToken() });
            }

            return NotFound();
        }

        private bool ValidateUser(UserLogin login)
        {
            return true;
        }

        private string GenerateToken()
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signingCredentials);
            //Claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,"Oscar Rueda"),
                new Claim(ClaimTypes.Email,"oscar_rueda@hotmail.com"),
                new Claim(ClaimTypes.Role,"Administrator")
            };

            //Payload
            var payload = new JwtPayload
            (
                _configuration["Authentication:SecretKey"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.Now,
                DateTime.Now.AddMinutes(5)
            );

            var token = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Exceptions;
using SocialMedia.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Sockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TransforSerPu.Core.Dtos;
using TransforSerPu.Core.Interfaces;

namespace SocialMedia.Core.Services
{
    public class TokenService : ITokenService
    {
        
        private readonly ISecurityService _securityService;
        private readonly AuthenticationOptions _authenticationOptions;


        private string issuer;
        private string audience;
        private string secret;

        public TokenService( ISecurityService securityService,
            IOptions<AuthenticationOptions> authenticationOptions)
        {
            _securityService = securityService;
            _authenticationOptions = authenticationOptions.Value;

            issuer = _authenticationOptions.Issuer;
            audience = _authenticationOptions.Audience;
            secret = _authenticationOptions.SecretKey;
        }

        

        public async Task<AuthenticatedResponse> RenewToken(TokenDto tokenApiModel)
        {
            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken, secret);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default

            var user = await _securityService.GetLoginByCredentials(new UserLoginDto() { User = username });

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new BusinessException("Invalid client request");

            var newAccessToken = GenerateAccessToken(principal.Claims, issuer, audience, secret);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _securityService.UpdateRefreshToken(username, newRefreshToken);

            return new AuthenticatedResponse()
            {
                AuthToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims, string issuer, string audience, string secret)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddHours(4),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secret)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}

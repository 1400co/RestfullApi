using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TransforSerPu.Core.Dtos;

namespace TransforSerPu.Core.Interfaces
{
    public interface ITokenService
    {
        Task<AuthenticatedResponse> RenewToken(TokenDto tokenApiModel);
        string GenerateAccessToken(IEnumerable<Claim> claims, string issuer, string audience, string secret);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string secret);
    }
}

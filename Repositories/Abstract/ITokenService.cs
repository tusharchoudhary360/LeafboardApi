using AuthApi.Models.Domain;
using AuthApi.Models.DTO;
using System.Security.Claims;

namespace AuthApi.Repositories.Abstract
{
    public interface ITokenService
    {
        TokenResponse GetToken(Users user, string userRoles);
        TokenResponse GetrefToken(IEnumerable<Claim> claim);
        string GetRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace HFSolutions.TestDotNet.Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
        }
    }
}

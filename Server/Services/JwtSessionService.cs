using Server.Interfaces;

namespace Server.Services;

public class JwtSessionService(IHttpContextAccessor httpContextAccessor) : ISessionService
{
    public string? GetClaim(string claimType)
    {
        var claimsPrincipal = httpContextAccessor.HttpContext?.User;

        if (claimsPrincipal == null || !claimsPrincipal.Identity!.IsAuthenticated)
        {
            return null;
        }

        return claimsPrincipal.FindFirst(claimType)?.Value;
    }
}

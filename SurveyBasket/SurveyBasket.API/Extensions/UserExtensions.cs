using System.Security.Claims;

namespace SurveyBasket.API.Extensions;

public static class UserExtensions
{
    public static string? GetUserId(this ClaimsPrincipal claims)
    {
        return claims.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}

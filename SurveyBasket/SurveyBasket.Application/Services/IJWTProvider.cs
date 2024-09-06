

namespace SurveyBasket.Application.Services;
public interface IJWTProvider
{
    (string token, int expiresIn) GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permessions);

    string? ValidateToken(string token);
}

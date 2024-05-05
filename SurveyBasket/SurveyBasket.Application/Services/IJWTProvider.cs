

namespace SurveyBasket.Application.Services;
public interface IJWTProvider
{
    (string token, int expiresIn) GenerateToken(ApplicationUser user);

    string? ValidateToken(string token);   
}

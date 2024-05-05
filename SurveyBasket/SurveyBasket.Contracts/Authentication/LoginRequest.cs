
namespace SurveyBasket.Contracts.Authentication;
public record LoginRequest(string Email , string Password);
public record RegisterRequest(string FirstName ,string LastName,string UserName,string Email , string Password);


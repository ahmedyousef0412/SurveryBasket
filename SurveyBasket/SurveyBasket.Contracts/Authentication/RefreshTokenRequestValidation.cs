

namespace SurveyBasket.Contracts.Authentication;
public class RefreshTokenRequestValidation : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidation()
    {
        RuleFor(x => x.Token).NotEmpty();
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}



namespace SurveyBasket.Contracts.Users;
public class CreateUserRequestValidator:AbstractValidator<CreateUserRequest>
{

    public CreateUserRequestValidator()
    {

        RuleFor(x => x.FirstName)
             .NotEmpty()
             .Length(3, 100);

        RuleFor(x => x.LastName)
             .NotEmpty()
             .Length(3, 100);

        RuleFor(x => x.Email)
             .NotEmpty()
             .EmailAddress();

        RuleFor(x => x.Password)
           .NotEmpty()
           .Matches(RegexPatterns.Password)
          .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

        RuleFor(u => u.Roles)
            .NotEmpty()
            .NotNull();

        RuleFor(u => u.Roles)
            .Must(x => x.Distinct().Count() == x.Count)
            .WithMessage("You can't add dublicated role for the same use")
            .When(u => u.Roles != null);
    }
}

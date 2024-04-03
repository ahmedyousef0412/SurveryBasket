
namespace SurveyBasket.Contracts.Validation;
public class CreatePollRequestValidator:AbstractValidator<CreatePollRequest>
{

    public CreatePollRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()

            .Length(5, 20)
            .WithMessage("{PropertyName}  should be at least {MinLength} and maximum {MaxLength} , you entered [{PropertyValue}]");


        RuleFor(x => x.Description)
            .NotEmpty()
            .Length(10, 1000)
            .WithMessage("{PropertyName} should be at least {MinLength} and maximum {MaxLength} , you entered [{PropertyValue}]");

    }
}

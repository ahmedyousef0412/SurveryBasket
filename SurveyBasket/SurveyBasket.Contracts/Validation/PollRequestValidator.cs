
namespace SurveyBasket.Contracts.Validation;
public class PollRequestValidator:AbstractValidator<PollRequest>
{

    public PollRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()

            .Length(5, 120)
            .WithMessage("{PropertyName}  should be at least {MinLength} and maximum {MaxLength} , you entered [{PropertyValue}]");


        RuleFor(x => x.Summary)
            .NotEmpty()
            .Length(10, 1000)
            .WithMessage("{PropertyName} should be at least {MinLength} and maximum {MaxLength} , you entered [{PropertyValue}]");



        RuleFor(x => x.StartsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));


        RuleFor(x => x.EndsAt)
          .NotEmpty();

        RuleFor(x => x)
            .Must(HasValideDates)
            .WithName(nameof(PollRequest.EndsAt))
            .WithMessage("{PropertyName} should be greater than or equal to startAt");
        
          
    }

    private bool HasValideDates(PollRequest pollRequest)
    {
        return pollRequest.EndsAt >= pollRequest.StartsAt;
    }
}

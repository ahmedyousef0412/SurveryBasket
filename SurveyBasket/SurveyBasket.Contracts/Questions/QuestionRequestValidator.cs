
namespace SurveyBasket.Contracts.Questions;
public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
{
    public QuestionRequestValidator()
    {
        RuleFor(q => q.Content)
            .NotEmpty()
            .Length(5, 1000)
            .WithMessage("{PropertyName}  should be at least {MinLength} and maximum {MaxLength} , you entered [{PropertyValue}]");

        RuleFor(q => q.Answers)
            .NotNull();

        RuleFor(q => q.Answers)
          .Must(q => q.Count > 1)
           .WithMessage("Question should at least has 2 answers")
           .When(q => q.Answers != null);


        RuleFor(q => q.Answers)

           .Must(q => q.Distinct().Count() == q.Count)
           .WithMessage("You can't add duplicated answers")
           .When(q => q.Answers != null); ;
    }
}

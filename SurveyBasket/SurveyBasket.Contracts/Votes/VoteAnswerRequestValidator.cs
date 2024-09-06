

namespace SurveyBasket.Contracts.Votes;
public class VoteAnswerRequestValidator : AbstractValidator<VoteAnswerRequest>
{
    public VoteAnswerRequestValidator()
    {
        RuleFor(var => var.QuestionId).GreaterThan(0);
        RuleFor(var => var.AnswerId).GreaterThan(0);
    }
}

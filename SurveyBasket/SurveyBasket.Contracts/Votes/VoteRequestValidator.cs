

namespace SurveyBasket.Contracts.Votes;
public class VoteRequestValidator:AbstractValidator<VoteRequest>
{
	public VoteRequestValidator()
	{
		RuleFor(vr => vr.Answers).NotEmpty();

		RuleForEach(vr => vr.Answers)
			.SetInheritanceValidator(v =>
			v.Add(new VoteAnswerRequestValidator()));
	}
}

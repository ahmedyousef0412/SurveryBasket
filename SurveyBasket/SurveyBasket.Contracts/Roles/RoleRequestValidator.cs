
namespace SurveyBasket.Contracts.Roles;
public class RoleRequestValidator : AbstractValidator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .Length(3, 256);

        RuleFor(r => r.Permissions)
           .NotEmpty()
           .NotNull();


        RuleFor(r => r.Permissions)
            .Must(r => r.Distinct().Count() == r.Count)
            .WithMessage("You can't add duplicated permissions for the same role")
            .When(r => r.Permissions != null);
    }
}



namespace SurveyBasket.Infrastruction.Implementations.Authentications.Filters;
public class PermissionRequirment(string permission):IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}

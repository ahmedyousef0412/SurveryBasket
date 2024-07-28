

namespace SurveyBasket.Infrastruction.Implementations.Authentications.Filters;
public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission) { }


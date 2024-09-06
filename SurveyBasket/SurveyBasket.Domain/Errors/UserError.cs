

namespace SurveyBasket.Domain.Errors;
public record UserError
{


    public static readonly Error UserNotFound =
     new("User.UserNotFound", "User is not found", StatusCodes.Status404NotFound);

    public static readonly Error InvalidCredentials =
        new("User.InvalidCredentials", "Invalid Email or Password", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidJwtToken =
      new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);


    public static readonly Error DuplicatedEmail =
       new("User.DuplicatedEmail", "Email is already exist", StatusCodes.Status409Conflict);

    public static readonly Error EmailNotConfirmed =
      new("User.EmailNotConfirmed", "Email is not confirmed ", StatusCodes.Status401Unauthorized);


    public static readonly Error InvalidCode =
      new("User.InvalidCode", "Invalid code", StatusCodes.Status401Unauthorized);


    public static readonly Error DuplicatedConfirmedEmail =
      new("User.DuplicatedConfirmedEmail", "Email is already confirmed", StatusCodes.Status400BadRequest);

    public static readonly Error DisabledUser =
       new("User.DisabledUser", "Disabled User, please contact with your administrator", StatusCodes.Status401Unauthorized);

    public static readonly Error LockedUser =
     new("User.LockedUser", "Locked User, please contact with your administrator", StatusCodes.Status401Unauthorized);


    public static readonly Error InvalidRoles =
           new("User.InvalidRoles", " Invalid Roles", StatusCodes.Status400BadRequest);
}

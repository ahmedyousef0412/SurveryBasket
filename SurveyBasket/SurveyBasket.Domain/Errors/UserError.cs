﻿

namespace SurveyBasket.Domain.Errors;
public static class UserError
{
    public static readonly Error InvalidCredentials = 
        new("User.InvalidCredentials", "Invalid Email or Password",StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidJwtToken =
      new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);
}

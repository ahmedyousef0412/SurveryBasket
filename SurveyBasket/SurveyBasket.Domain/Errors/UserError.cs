using Microsoft.AspNetCore.Http;
using SurveyBasket.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyBasket.Domain.Errors;
public static class UserError
{
    public static readonly Error InvalidCredentials = 
        new("User.InvalidCredentials", "Invalid Email or Password",StatusCodes.Status400BadRequest);

    public static readonly Error InvalidJwtToken =
      new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status400BadRequest);
}

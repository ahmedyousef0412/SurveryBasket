

using Microsoft.AspNetCore.Http;

namespace SurveyBasket.Domain.Errors;
public static  class PollErrors
{

    public static readonly Error PollNotFound =
         new("Poll.NotFound", "No poll was found with the given ID" , StatusCodes.Status400BadRequest);


}

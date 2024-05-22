


namespace SurveyBasket.API.Controllers;


[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;


    //[HttpPost("Register")]
    //public async Task<IActionResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    //{
    //    var authResult = await _authService.RegisterAsync(request.FirstName,request.LastName,request.UserName ,request.Email,request.Password ,cancellationToken);

    //    return authResult is null ? BadRequest("Invalid Email or Password") : Ok(authResult);
    //}

    [HttpPost("")]

    public async Task<IActionResult> LoginAsync([FromBody]LoginRequest request ,CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetTokenAsync(request.Email , request.Password , cancellationToken);

        return authResult.IsSuccess
            ? Ok(authResult.Value)
            : authResult.ToProblem();

    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody]RefreshTokenRequest request ,CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token ,request.RefreshToken , cancellationToken);

      
        return authResult.IsSuccess
            ? Ok(authResult.Value)
            : authResult.ToProblem();
    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return result.IsSuccess
           ? Ok()
           : result.ToProblem();

    }
    private BadRequestObjectResult CreateBadRequestProblemDetailsResponse(Error error)
    {
        var problemDetails = new ProblemDetails()
        {
            Status = StatusCodes.Status400BadRequest,
            Title = ErrorTitles.BadRequest,
            Extensions =
                {
                    ["errors"]= new []{ error }
                }
        };

        return BadRequest(problemDetails);
    }
}

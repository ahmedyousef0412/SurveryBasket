
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

        return authResult is null ? BadRequest("Invalid Email or Password") : Ok(authResult);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody]RefreshTokenRequest request ,CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token ,request.RefreshToken , cancellationToken);

        return authResult is null ? BadRequest("Invalid Token") : Ok(authResult);
    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var isRevoked = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return isRevoked ? Ok() : BadRequest("Operation Failed !");
    }
}

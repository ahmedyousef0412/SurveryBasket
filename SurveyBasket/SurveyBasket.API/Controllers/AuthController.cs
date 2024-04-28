


namespace SurveyBasket.API.Controllers;
[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;


    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.RegisterAsync(request.FirstName,request.LastName,request.UserName ,request.Email,request.Password ,cancellationToken);

        return authResult is null ? BadRequest("Invalid Email or Password") : Ok(authResult);
    }

    [HttpPost("")]

    public async Task<IActionResult> LoginAsync(LoginRequest request ,CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetTokenAsync(request.Email , request.Password , cancellationToken);

        return authResult is null ? BadRequest("Invalid Email or Password") : Ok(authResult);
    }
}

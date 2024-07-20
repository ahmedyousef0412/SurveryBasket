﻿

namespace SurveyBasket.API.Controllers;


[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody]RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RgisterAsync(request, cancellationToken);

        return result.IsSuccess ? Ok() : result.ToProblem();
    }



    [HttpPost("")]
    public async Task<IActionResult> Login([FromBody]LoginRequest request ,CancellationToken cancellationToken)
    {
        var result = await _authService.GetTokenAsync(request.Email , request.Password , cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();

    }



    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody]RefreshTokenRequest request ,CancellationToken cancellationToken)
    {
        var result = await _authService.GetRefreshTokenAsync(request.Token ,request.RefreshToken , cancellationToken);

      
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }



    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.ConfirmEmailAsync(request, cancellationToken);


        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }




    [HttpPost("resend-confirm-email")]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest request)
    {
        var result = await _authService.ResendConfirmationEmailAsync(request);


        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }




    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
    {
        var result = await _authService.SendResetPasswordCodeAsync(request);


        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }



    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authService.ResetPasswordAsync(request);


        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }



    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return result.IsSuccess
           ? Ok()
           : result.ToProblem();

    }
   
  
}

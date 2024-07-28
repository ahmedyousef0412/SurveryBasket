

namespace SurveyBasket.Infrastruction.Implementations;
internal class AuthService
    (
         UserManager<ApplicationUser> userManager,
         SignInManager<ApplicationUser> signInManager,
         IJWTProvider jWTProvider,
         ILogger<AuthService> logger,
         IEmailSender emailSender,
         IHttpContextAccessor httpContextAccessor,
         ApplicationDbContext context
    ) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IJWTProvider _jwtProvider = jWTProvider;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ApplicationDbContext _context = context;
    private readonly int _refreshTokenExpiryDays = 14;



    public async Task<Result> RgisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var emailIsExists = await _userManager.Users.AnyAsync(u => u.Email == request.Email,cancellationToken);

        if (emailIsExists)
            return Result.Failure(UserError.DuplicatedEmail);

        var user = request.Adapt<ApplicationUser>();


        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Confirmation code : {code}", code);

         

            await SendConfirmationEmail(user, code);

            return Result.Success();
        }

        var errors = result.Errors.First();

        return Result.Failure(new Error(errors.Code, errors.Description, StatusCodes.Status400BadRequest));


    }

    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {

        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return Result.Failure<AuthResponse>(UserError.InvalidCredentials);

        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
       
        if (result.Succeeded)
        {
           
            var response = await GenerateAuthResponseAsync(user,cancellationToken);

            return Result.Success(response);
        }

        return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserError.EmailNotConfirmed : UserError.InvalidCredentials);
       

    }
    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken = default)
    {

        //Find the uesr
        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
            return Result.Failure(UserError.InvalidCode);

        //Check if user is already confirmed
        if(user.EmailConfirmed)
            return Result.Failure(UserError.DuplicatedConfirmedEmail);

        var code = request.Code;

        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {

            return Result.Failure(UserError.InvalidCode);
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
        {

            await _userManager.AddToRoleAsync(user, DefaultRoles.Member);
            return Result.Success();
        }

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Result.Success();


        if (user.EmailConfirmed)
            return Result.Failure(UserError.DuplicatedConfirmedEmail);

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        _logger.LogInformation("Confirmation code : {code}", code);


        await SendConfirmationEmail(user, code);

        return Result.Success();
    }


    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        //06sjhkfoiwfhgww88w9dd
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure<AuthResponse>(UserError.InvalidJwtToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure<AuthResponse>(UserError.InvalidJwtToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(u => u.Token == refreshToken && u.IsActive);

        if (userRefreshToken is null) 
            return Result.Failure<AuthResponse>(UserError.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var (roles, permessions) = await GetUserRolesAndPermessions(user, cancellationToken);


        var (newToken, expiresOn) = _jwtProvider.GenerateToken(user,roles,permessions);

        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiresOn = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {

            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiresOn
        });

        await _userManager.UpdateAsync(user);

        var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresOn, newRefreshToken, refreshTokenExpiresOn);


        return Result.Success(response);

    }


    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null) 
            return Result.Failure(UserError.InvalidJwtToken);

        var user =await _userManager.FindByIdAsync(userId);

        if (user is null) 
            return Result.Failure(UserError.InvalidJwtToken); ;


        var userRefreshToken = user.RefreshTokens
            .SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if(userRefreshToken is null)
            return Result.Failure(UserError.InvalidRefreshToken); 

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }

   
    public async Task<Result> SendResetPasswordCodeAsync(ForgetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null || !user.EmailConfirmed)
            return Result.Failure(UserError.EmailNotConfirmed);


        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));


        _logger.LogInformation("Reset code : {code}", code);


        await SendResetPasswordEmail(user, code);

        return Result.Success();

    }


    public async Task<Result>ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null || !user.EmailConfirmed)
            return Result.Failure(UserError.InvalidCode);

        IdentityResult result;

        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));

            result = await _userManager.ResetPasswordAsync(user, code ,request.NewPassword);
        }
        catch (FormatException)
        {

            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));


    }


    #region Helper Method
    private async Task<AuthResponse> GenerateAuthResponseAsync(ApplicationUser user,CancellationToken cancellationToken)
    {

        var (roles, permessions) = await GetUserRolesAndPermessions(user, cancellationToken);

        // Generate the JWT token and expiration time
        var (token, expiresIn) = _jwtProvider.GenerateToken(user,roles,permessions);

        // Generate the refresh token and its expiration date
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiresOn = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        // Add the refresh token to the user's refresh tokens list
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiresOn,
        });

        // Update the user in the database
        await _userManager.UpdateAsync(user);

        // Create and return the authentication response
        return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiresOn);
    }


    private async Task SendConfirmationEmail(ApplicationUser user, string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",

            templateModel: new Dictionary<string, string>
            {
                { "{{name}}", user.FirstName },
                { "{{action_url}}", $"{origin}/Auth/emailConfirmation?userId={user.Id}&code={code}" }
            }
        );

        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ Survey Basket: Email Confirmation", emailBody));

        await Task.CompletedTask;
    }

    private async Task SendResetPasswordEmail(ApplicationUser user, string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
            templateModel: new Dictionary<string, string>
            {
                { "{{name}}", user.FirstName },
                { "{{action_url}}", $"{origin}/auth/forgetPassword?email={user.Email}&code={code}" }
            }
        );

        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ Survey Basket: Change Password", emailBody));

        await Task.CompletedTask;
    }
   
    
    private static string GenerateRefreshToken()
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    private async Task<(IEnumerable<string> roles,IEnumerable<string> permessions)> GetUserRolesAndPermessions(ApplicationUser user ,CancellationToken cancellationToken)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        var userPermessions = await 
                                     (
                                         from r in _context.Roles
                                         join rc in _context.RoleClaims
                                         on r.Id equals rc.RoleId
                                         where userRoles.Contains(r.Name!)
                                         select rc.ClaimValue!
                                     )
                                     .Distinct()
                                     .ToListAsync(cancellationToken);


        return (userRoles, userPermessions);
    }

    #endregion
}

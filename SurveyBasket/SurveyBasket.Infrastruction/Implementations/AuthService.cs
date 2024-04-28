

using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.Infrastruction.Implementations;
internal class AuthService(UserManager<ApplicationUser> userManager,IJWTProvider jWTProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJWTProvider _jwtProvider = jWTProvider;




    public async Task<AuthResponse> RegisterAsync(string firstName, string lastName, string userName, string email, string password, CancellationToken cancellationToken = default)
    {
        if (await _userManager.FindByEmailAsync(email) is not null)
            return null;

        if (await _userManager.FindByNameAsync(userName) is not null)
            return null;

        var user = new ApplicationUser
        {
            UserName = userName,
            Email = email,
            FirstName = firstName,
            LastName = lastName
        };

        await _userManager.CreateAsync(user, password);

        var (token, expirsIn) = _jwtProvider.GenerateToken(user);


        return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expirsIn);
    }


    public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        //var user = await _userManager.FindByEmailAsync(email) ?? _userManager.FindByNameAsync(userName);

        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return null;

       var isValidPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!isValidPassword)
            return null;

        var (token, expirsIn) = _jwtProvider.GenerateToken(user);


        return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expirsIn);

    }

}



using System.Security.Cryptography;

namespace SurveyBasket.Infrastruction.Implementations;
internal class AuthService(UserManager<ApplicationUser> userManager,IJWTProvider jWTProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJWTProvider _jwtProvider = jWTProvider;
    private readonly int _refreshTokenExpiryDays = 14;



    //public async Task<AuthResponse> RegisterAsync(string firstName, string lastName, string userName, string email, string password, CancellationToken cancellationToken = default)
    //{
    //    if (await _userManager.FindByEmailAsync(email) is not null)
    //        return null;

    //    if (await _userManager.FindByNameAsync(userName) is not null)
    //        return null;

    //    var user = new ApplicationUser
    //    {
    //        UserName = userName,
    //        Email = email,
    //        FirstName = firstName,
    //        LastName = lastName
    //    };

    //    await _userManager.CreateAsync(user, password);

    //    var (token, expirsIn) = _jwtProvider.GenerateToken(user);


    //    return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expirsIn);
    //}


    public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {


        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return null;

       var isValidPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!isValidPassword)
            return null;

        var (token, expiresIn) = _jwtProvider.GenerateToken(user);

        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiresOn = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiresOn,
        });

        await _userManager.UpdateAsync(user);

        return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken ,refreshTokenExpiresOn);

    }
  
    public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        //06sjhkfoiwfhgww88w9dd
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null) return null;

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null) return null;

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(u => u.Token == refreshToken && u.IsActive);

        if (userRefreshToken is null) return null;

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        var (newToken, expiresOn) = _jwtProvider.GenerateToken(user);

        var newRefreshToken = GenerateRefreshToken();
        var refreshTokenExpiresOn = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {

            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiresOn
        });

        await _userManager.UpdateAsync(user);


        return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresOn, newRefreshToken, refreshTokenExpiresOn);

    }


    public async Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null) return false;

        var user =await _userManager.FindByIdAsync(userId);

        if (user is null) return false;


        var userRefreshToken = user.RefreshTokens
            .SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if(userRefreshToken is null) return false;

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return true;
    }



    private static string GenerateRefreshToken()
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

   
}

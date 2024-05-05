﻿
namespace SurveyBasket.Infrastruction.Implementations;

internal class JWTProvider(IOptions<JwtConfiguration> jwtConfiguration) : IJWTProvider
{

     private readonly JwtConfiguration _jwtConfiguration = jwtConfiguration.Value;

    public (string token, int expiresIn) GenerateToken(ApplicationUser user)
    {
        //Set Claims
        Claim[] claims = [
             new (JwtRegisteredClaimNames.Sub , user.Id),
             new (JwtRegisteredClaimNames.Email , user.Email!),
             new (JwtRegisteredClaimNames.GivenName , user.FirstName),
             new (JwtRegisteredClaimNames.FamilyName , user.LastName),
             new (JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString())

       ];

        // Encryption to Decoding and Encoding
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key));


        var signingCredentials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256);

        //var expiresIn = _jwtConfiguration.ExpireInMinute;


        // Token Shape
        var jwtSecurityToken = new JwtSecurityToken(

            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpireInMinute),
            signingCredentials: signingCredentials

            );
        //var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return (token: new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken), expiresIn: _jwtConfiguration.ExpireInMinute * 60);
    }

    public string? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();


        //Key that I used it to Encode I need it to Decode
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key));

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = symmetricSecurityKey,
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ClockSkew = TimeSpan.Zero

            }, out SecurityToken validatedToken) ;

            var jwtToken = (JwtSecurityToken)validatedToken;

            //Find UserId that return from Claims

            return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
        }
        catch 
        {

            return null;
        }

    }
}
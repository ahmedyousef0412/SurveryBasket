using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.Infrastruction.EntitiesConfiguration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
}



using SurveyBasket.Application.Services.Caching;
using SurveyBasket.Infrastruction.Implementations.Caching;

namespace SurveyBasket.Infrastruction.ConfigureServices;

public static class InfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region Polls

        services.AddScoped<IPollServices, PollService>();

        #endregion


        #region Auth

        services.AddScoped<IAuthService, AuthService>();

        #endregion

        #region Question

        services.AddScoped<IQuestionService, QuestionService>();

        #endregion

        #region Votes

        services.AddScoped<IVoteServices, VoteService>();

        #endregion

        #region Result

        services.AddScoped<IResultService, ResultService>();

        #endregion

        #region Identity

        services.AddIdentity<ApplicationUser, IdentityRole>()
         .AddEntityFrameworkStores<ApplicationDbContext>();

        #endregion

        #region  Caching

        services.AddScoped<ICacheService, CacheService>();

        #endregion

        #region JWT



        services.AddSingleton<IJWTProvider, JWTProvider>();



        services.AddOptions<JwtConfiguration>()
            .BindConfiguration(JwtConfiguration.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var jwtSettings = configuration.GetSection(JwtConfiguration.SectionName)
                                                     .Get<JwtConfiguration>();



        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
       .AddJwtBearer(o =>
       {
           o.SaveToken = true;
           o.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuerSigningKey = true,
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
               ValidIssuer = jwtSettings?.Issuer,
               ValidAudience = jwtSettings?.Audience,
               ClockSkew = TimeSpan.Zero
           };
       });

        #endregion





        return services;
    }
}

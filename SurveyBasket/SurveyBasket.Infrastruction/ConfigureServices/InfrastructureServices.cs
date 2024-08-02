


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


        services.Configure<IdentityOptions>(options =>
        {

            options.Password.RequiredLength = 8;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;
        });

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


        #region Roles

        services.AddScoped<IRoleService, RoleService>();
        #endregion
        #region Account Management

        services.AddScoped<IUserService, UserService>();

        #endregion
        #region Identity

        services.AddIdentity<ApplicationUser, ApplicationRole>()
         .AddEntityFrameworkStores<ApplicationDbContext>()
         .AddDefaultTokenProviders();

        #endregion

        #region  Caching

        services.AddScoped<ICacheService, CacheService>();

        #endregion

        #region JWT


        //Because I need only one instance 
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

        #region MailSetting

        services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));


        services.AddScoped<IEmailSender, EmailService>();


        #endregion

        #region Notifications

        services.AddScoped<INotificationService, NotificationService>();


        #endregion

        #region Permissions

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizeHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        #endregion

        #region Hangfire


        services.AddHangfire(config => config
      .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
      .UseSimpleAssemblyNameTypeSerializer()
      .UseRecommendedSerializerSettings()
      .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

        
        services.AddHangfireServer();

        #endregion


        services.AddHttpContextAccessor();

        return services;
    }
}

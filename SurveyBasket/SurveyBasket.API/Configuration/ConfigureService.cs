
namespace SurveyBasket.API.Configuration;

public static class ConfigureService
{

  
    public static IServiceCollection SurveyBasketApiDependeciesService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDistributedMemoryCache();

        services.AddConnectionString(configuration);

        services.AddControllers();

        services.AddSwaggerConfig().AddMapsterConfig();

        services.ApplyCORS(configuration);

        services.AddContract();


        services.AddInfrastructureServices(configuration);


        services.AddExceptionHandler<GlobalExceptionHandler>();

        services.AddProblemDetails();


        services.ApplyHealthCheck(configuration);

        services.ApplyRateLimiting();

        return services;
    }

    private static IServiceCollection AddConnectionString(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
           ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found !");


        services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(connectionString));


        return services;
    }
    
    private static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
    {

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    private static IServiceCollection ApplyCORS(this IServiceCollection services ,IConfiguration configuration)
    {
        services.AddCors(options =>
                   options.AddDefaultPolicy
                   (builder =>
                    builder.AllowAnyHeader().AllowAnyMethod()
                   .WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!)
                    )
        );

        return services;
    }
  
    private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        var mapConfig = TypeAdapterConfig.GlobalSettings;

        mapConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(mapConfig));


        return services;
    }

    private static IServiceCollection ApplyRateLimiting(this IServiceCollection services)
    {
        #region Concurency Mode

        //services.AddRateLimiter(rateLimiterOptions =>
        //{
        //    rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;


        //    rateLimiterOptions.AddConcurrencyLimiter("concurency", options =>
        //    {
        //        options.PermitLimit = 2;
        //        options.QueueLimit = 1;
        //        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;

        //    });
        //});

        #endregion


        #region Token Bucket
        //services.AddRateLimiter(rateLimiterOptions =>
        //{
        //    rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;


        //    rateLimiterOptions.AddTokenBucketLimiter("token", options =>
        //    {
        //        options.TokenLimit = 2;
        //        options.QueueLimit = 1;
        //        options.ReplenishmentPeriod = TimeSpan.FromSeconds(60);
        //        options.TokensPerPeriod = 2;
        //        options.AutoReplenishment = true; // Every 10 second (ReplenishmentPeriod )
        //                                          // , If the bucket is not full will be  generate tokens based on (TokensPerPeriod)
        //        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;

        //    });
        //});

        #endregion

        #region Fixed Window
        //services.AddRateLimiter(rateLimiterOptions =>
        //{
        //    rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;


        //    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
        //    {
        //        options.PermitLimit = 2;
        //        options.QueueLimit = 1;
        //        options.Window = TimeSpan.FromSeconds(60);
        //        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;

        //    });
        //});

        #endregion


      


        #region RateLimiter ,  Ip Limit and  User Limit


        services.AddRateLimiter(rateLimiterOptions =>
        {

            
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            //Concurency Mode

            rateLimiterOptions.AddConcurrencyLimiter(Policies.Concurency, options =>
            {
                   options.PermitLimit = 500;
                   options.QueueLimit = 50;
                   options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;

            });

            //Ip Limit
            rateLimiterOptions.AddPolicy(Policies.IpLimit, httpContext =>

                RateLimitPartition.GetFixedWindowLimiter(

                      partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                      factory: _ => new FixedWindowRateLimiterOptions
                      {
                          PermitLimit = 5,
                          Window = TimeSpan.FromSeconds(30)
                      }
                )
            );

             rateLimiterOptions.AddPolicy(Policies.UserLimit, httpContext =>

                   RateLimitPartition.GetFixedWindowLimiter(

                      partitionKey: httpContext.User.GetUserId(),
                      factory: _ => new FixedWindowRateLimiterOptions
                      {
                          PermitLimit = 3,
                          Window = TimeSpan.FromSeconds(30)
                      }
                   )
            );
        });
        #endregion

        return services;
    }

    private static IServiceCollection ApplyHealthCheck(this IServiceCollection services ,IConfiguration configuration)
    {
        services.AddHealthChecks()
           .AddSqlServer(name: "database", connectionString: configuration.GetConnectionString("DefaultConnection")!)
           .AddHangfire(options =>
           {
               options.MinimumAvailableServers = 1;
           })
           .AddCheck<MailProviderHealthCheck>(name: "mail provider");
        return services;
    }
}

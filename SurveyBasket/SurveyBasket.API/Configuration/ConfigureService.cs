
using SurveyBasket.API.Middleware;

namespace SurveyBasket.API.Configuration;

public static class ConfigureService
{

    public static IServiceCollection SurveyBasketApiDependeciesService(this IServiceCollection services,
        IConfiguration configuration)
    {


        services.AddDistributedMemoryCache();

        services.AddConnectionString(configuration);

        services.AddControllers();

        services.AddCors(options =>
                     options.AddDefaultPolicy
                     (builder =>
                      builder.AllowAnyHeader().AllowAnyMethod()
                     .WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!)
                      )
        );

        services.AddSwaggerConfig().AddMapsterConfig();

        services.AddContract();


        services.AddInfrastructureServices(configuration);


        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    private static IServiceCollection AddConnectionString(this IServiceCollection services,
        IConfiguration configuration)
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

    private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    {
        var mapConfig = TypeAdapterConfig.GlobalSettings;

        mapConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(mapConfig));


        return services;
    }
}

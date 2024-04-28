
namespace SurveyBasket.API.Configuration;

public static class ConfigureService
{

    public static IServiceCollection SurveyBasketApiDependeciesService(this IServiceCollection services,
        IConfiguration configuration)
    {


        services.AddConnectionString(configuration);

        services.AddControllers();

        services.AddSwaggerConfig().AddMapsterConfig();

        services.AddContract();


        services.AddInfrastructureServices(configuration);


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

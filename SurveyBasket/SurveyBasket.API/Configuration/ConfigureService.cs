
namespace SurveyBasket.API.Configuration;

public static class ConfigureService
{

    public static IServiceCollection SurveyBasketApiDependeciesService(this IServiceCollection services,
        IConfiguration configuration)
    {


        services.AddConnectionString(configuration);

        services.AddControllers();

        services.AddSwaggerService().AddMapsterService();

        services.AddContract();


        services.AddInfrastructureServices();


        return services;
    }

    public static IServiceCollection AddConnectionString(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
           ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found !");


        services.AddDbContext<ApplicationDbContext>(options =>
          options.UseSqlServer(connectionString));


        return services;
    }
    public static IServiceCollection AddSwaggerService(this IServiceCollection services)
    {

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static IServiceCollection AddMapsterService(this IServiceCollection services)
    {
        var mapConfig = TypeAdapterConfig.GlobalSettings;

        mapConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(mapConfig));


        return services;
    }
}

using MapsterMapper;
using SurveyBasket.Contracts.Configurations;
using SurveyBasket.Infrastruction.ConfigureServices;
using System.Reflection;

namespace SurveyBasket.API.Configuration;

public static class ConfigureService
{

    public static IServiceCollection SurveyBasketApiDependeciesService(this IServiceCollection services)
    {

        services.AddControllers();

        services.AddSwaggerService().AddMapsterService();


       
        services.AddContract();


        services.AddInfrastructureServices();


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

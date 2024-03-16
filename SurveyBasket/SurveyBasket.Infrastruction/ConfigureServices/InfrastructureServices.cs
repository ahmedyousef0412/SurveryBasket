
namespace SurveyBasket.Infrastruction.ConfigureServices;

public static class InfrastructureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        #region Polls

        services.AddScoped<IPollServices, PollService>();
        #endregion
        return services;
    }
}

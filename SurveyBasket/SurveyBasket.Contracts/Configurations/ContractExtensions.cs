﻿



namespace SurveyBasket.Contracts.Configurations;
public static class ContractExtensions
{
    public static IServiceCollection AddContract(this IServiceCollection services)
    {
        #region Fluent Validation

        services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());



        #endregion

        return services;

    }
}

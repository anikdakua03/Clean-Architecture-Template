using DinnerBooking.Application.Common.Interfaces.Authentication;
using DinnerBooking.Application.Common.Interfaces.Persistance;
using DinnerBooking.Application.Common.Interfaces.Services;
using DinnerBooking.Infrastructure.Authentication;
using DinnerBooking.Infrastructure.Persistance;
using DinnerBooking.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DinnerBooking.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
};
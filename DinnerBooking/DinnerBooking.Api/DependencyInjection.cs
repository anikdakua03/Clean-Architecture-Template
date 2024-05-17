using DinnerBooking.Api.Common.Errors;
using DinnerBooking.Api.Common.Mapping;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace DinnerBooking.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddMappings();
        // builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());
        services.AddControllers();

        services.AddSingleton<ProblemDetailsFactory, DinnerBookingProblemDetailsFactory>();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}
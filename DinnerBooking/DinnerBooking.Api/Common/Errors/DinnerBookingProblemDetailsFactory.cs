using System.Diagnostics;
using DinnerBooking.Api.Http;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;

namespace DinnerBooking.Api.Common.Errors;
public class DinnerBookingProblemDetailsFactory : ProblemDetailsFactory
{
    private readonly ApiBehaviorOptions _options;
    private readonly Action<ProblemDetailsContext>? _configure;

    public DinnerBookingProblemDetailsFactory(
        IOptions<ApiBehaviorOptions> options
    , IOptions<ProblemDetailsOptions>? problemDetailsOptions = null
    )
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        _configure = problemDetailsOptions?.Value?.CustomizeProblemDetails;
    }

    public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
    {
        statusCode ??= 500;

        var problemDetails = new ProblemDetails
        {
            Type = type,
            Title = title,
            Status = statusCode,
            // Detail = detail,
            // Instance = instance
        };

        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

        return problemDetails;
    }

    public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext, ModelStateDictionary modelStateDictionary, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
    {
        ArgumentNullException.ThrowIfNull(modelStateDictionary);

        statusCode ??= 400;

        var problemDetails = new ValidationProblemDetails
        {
            Type = type,
            Status = statusCode,
            // Detail = detail,
            // Instance = instance
        };

        if (title != null)
        {
            problemDetails.Title = title;
        }

        ApplyProblemDetailsDefaults(httpContext, problemDetails, statusCode.Value);

        return problemDetails;

    }

    private void ApplyProblemDetailsDefaults(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
    {
        problemDetails.Status ??= statusCode;

        if (_options.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData))
        {
            problemDetails.Title ??= clientErrorData.Title;
            problemDetails.Type ??= clientErrorData.Link;
        }

        var traceId = Activity.Current?.Id ?? httpContext?.TraceIdentifier;

        if (traceId != null)
        {
            problemDetails.Extensions["traceId"] = traceId;
        }

        // Our defaults as required
        var errors = httpContext?.Items[HttpContextItemKeys.Errors] as List<Error>;

        if (errors is not null)
        {
            var errorDictionary = new Dictionary<string, List<string>>();

            foreach (var error in errors)
            {
                if (errorDictionary.ContainsKey(error.Code))
                {
                    errorDictionary[error.Code].Add(error.Description);
                }
                else
                {
                    errorDictionary.Add(error.Code, new List<string> { error.Description });
                }
            }

            problemDetails.Extensions.Add(HttpContextItemKeys.Errors, errorDictionary);
        }

        // _configure?.Invoke(new() { HttpContext = httpContext!, ProblemDetails = problemDetails });
    }
}
using ErrorOr;
using FluentValidation;
using MediatR;

namespace DinnerBooking.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationBehavior(IValidator<TRequest>? validator = null)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if(_validator is null)
        {
            return await next();
        }
        
        var validationResult = await _validator.ValidateAsync(request);

        if (validationResult.IsValid)
        {
            return await next();
        }

        var errors = validationResult.Errors.ConvertAll(validationFailure => Error.Validation(
            validationFailure.PropertyName, validationFailure.ErrorMessage));

        // using dynamic to convert List of Error to TResponse type , we intentionally used this for conversion as there no such still exists and also we are sure of tha we will only get List of errors for converting to TResponse type
        return (dynamic)errors;
    }
}
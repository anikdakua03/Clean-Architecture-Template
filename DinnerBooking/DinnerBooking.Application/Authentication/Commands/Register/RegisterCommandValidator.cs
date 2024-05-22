using FluentValidation;

namespace DinnerBooking.Application.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MinimumLength(4);
        RuleFor(x => x.LastName).NotEmpty().MinimumLength(4);
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}
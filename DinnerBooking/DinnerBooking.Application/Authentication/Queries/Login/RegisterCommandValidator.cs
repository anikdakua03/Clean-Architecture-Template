using DinnerBooking.Application.Authentication.Queries.Login;
using FluentValidation;

namespace DinnerBooking.Application.Authentication.Commands.Login;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}
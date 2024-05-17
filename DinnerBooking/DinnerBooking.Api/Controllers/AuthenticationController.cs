using DinnerBooking.Application.Authentication.Commands.Register;
using DinnerBooking.Application.Authentication.Queries.Login;
using DinnerBooking.Application.Services.Authentication.Common;
using DinnerBooking.Contracts.Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DinnerBooking.Api.Controllers;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;

    public AuthenticationController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest registerRequest)
    {
        var command = new RegisterCommand(registerRequest.FirstName, registerRequest.LastName, registerRequest.Email, registerRequest.Password);

        var authResult = await _mediator.Send(command);

        return authResult.Match(
            authResult => Ok(NewMethod(authResult)),
            errors => Problem(errors));
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
    {
        var query = new LoginQuery(loginRequest.Email, loginRequest.Password);

        var authResult = await _mediator.Send(query);

        return authResult.Match(
                authResult => Ok(NewMethod(authResult)),
                errors => Problem(errors));
    }

    private static AuthenticationResponse NewMethod(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
                    authResult.User.Id,
                    authResult.User.FirstName,
                    authResult.User.LastName,
                    authResult.User.Email,
                    authResult.Token);
    }
}
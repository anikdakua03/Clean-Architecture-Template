using DinnerBooking.Application.Authentication.Commands.Register;
using DinnerBooking.Application.Authentication.Queries.Login;
using DinnerBooking.Application.Services.Authentication.Common;
using DinnerBooking.Contracts.Authentication;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DinnerBooking.Api.Controllers;

[Route("auth")]
[AllowAnonymous]
public class AuthenticationController : ApiController
{
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AuthenticationController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegisterRequest registerRequest)
    {
        var command = _mapper.Map<RegisterCommand>(registerRequest);

        var authResult = await _mediator.Send(command);

        return authResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(errors));
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
    {
        var query = _mapper.Map<LoginQuery>(loginRequest);

        var authResult = await _mediator.Send(query);

        return authResult.Match(
                authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
                errors => Problem(errors));
    }

    // private static AuthenticationResponse NewMethod(AuthenticationResult authResult)
    // {
    //     return new AuthenticationResponse(
    //                 authResult.User.Id,
    //                 authResult.User.FirstName,
    //                 authResult.User.LastName,
    //                 authResult.User.Email,
    //                 authResult.Token);
    // }
}
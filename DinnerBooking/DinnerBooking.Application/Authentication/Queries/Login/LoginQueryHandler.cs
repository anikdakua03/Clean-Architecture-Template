using DinnerBooking.Application.Common.Interfaces.Authentication;
using DinnerBooking.Application.Common.Interfaces.Persistance;
using DinnerBooking.Application.Services.Authentication.Common;
using DinnerBooking.Domain.Common.Errors;
using DinnerBooking.Domain.Entities;
using ErrorOr;
using MediatR;

namespace DinnerBooking.Application.Authentication.Queries.Login;

public class LoginQueryHandler : 
    IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask; // for async warning handling

        // 1. Validate user exists
        if (_userRepository.GetUserByEmail(query.Email) is not User user)
        {
            return Errors.Authentication.InvalidCredentials;
        }

        // 2. Validate the password
        if (user.Password != query.Password)
        {
            return new[] { Errors.Authentication.InvalidCredentials };
        }

        // 3.  Create JWT token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}
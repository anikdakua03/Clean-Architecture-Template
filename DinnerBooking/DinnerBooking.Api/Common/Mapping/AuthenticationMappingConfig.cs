using DinnerBooking.Application.Authentication.Commands.Register;
using DinnerBooking.Application.Authentication.Queries.Login;
using DinnerBooking.Application.Services.Authentication.Common;
using DinnerBooking.Contracts.Authentication;
using Mapster;

namespace DinnerBooking.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();
        
        config.NewConfig<LoginRequest, LoginQuery>();

        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            // .Map(dest => dest.Token, src => src.Token) // no need as they are same
            .Map(dest => dest, src => src.User);
    }
}
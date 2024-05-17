using DinnerBooking.Domain.Entities;

namespace DinnerBooking.Application.Services.Authentication.Common;
public record AuthenticationResult(
    User User,
    string Token
);
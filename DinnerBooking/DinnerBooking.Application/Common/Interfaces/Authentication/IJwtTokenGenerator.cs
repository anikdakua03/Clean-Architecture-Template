using DinnerBooking.Domain.Entities;

namespace DinnerBooking.Application.Common.Interfaces.Authentication;
public interface IJwtTokenGenerator
{
    string GenerateToken(User User);
}
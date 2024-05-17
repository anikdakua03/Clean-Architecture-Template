

using DinnerBooking.Application.Common.Interfaces.Services;

namespace DinnerBooking.Infrastructure.Services;
public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
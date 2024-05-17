using DinnerBooking.Application.Common.Interfaces.Persistance;
using DinnerBooking.Domain.Entities;

namespace DinnerBooking.Infrastructure.Persistance;
public class UserRepository : IUserRepository
{
    // temporary
    private static readonly List<User> _users = new List<User>();

    public void Add(User user)
    {
        _users.Add(user);
    }

    public User? GetUserByEmail(string email)
    {
        return _users.SingleOrDefault(a => a.Email == email);
    }
}
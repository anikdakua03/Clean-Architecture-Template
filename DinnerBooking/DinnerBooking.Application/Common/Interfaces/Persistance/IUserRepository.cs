
using DinnerBooking.Domain.Entities;

namespace DinnerBooking.Application.Common.Interfaces.Persistance;
    public interface IUserRepository
    {
        User? GetUserByEmail(string email);
        void Add(User user);
    }
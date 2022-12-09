using Boilerplate.Domain.Models;

namespace Boilerplate.Domain.Interfaces;

public interface IAuthUsersRepository
{
    Task<UserModel> GetUserAsync(string username, string password);
}
using KeyManager.Domain.Models;

namespace KeyManager.Domain.Interfaces;

public interface IAuthUsersRepository
{
    Task<UserModel> GetUserAsync(string username, string password);
}
using System.Diagnostics.CodeAnalysis;

namespace Boilerplate.Domain.Models;

public class UserConstants : IAuthUsersRepository
{
    [SuppressMessage("SonarLint", "S2068", Justification = "Ignored intentionally as a boilerplate app")]
    private readonly List<UserModel> _userCollection = new()
    {
        new UserModel { Username = "ckaraboran", Password = "ckaraboran", Role = "Admin" }
    };
    public Task<UserModel> GetUserAsync(string username, string password)
    {
        return Task.FromResult(_userCollection.FirstOrDefault(x => x.Username.ToLower() == username.ToLower() 
                                                                   && x.Password.ToLower() == password.ToLower()));
    }
}
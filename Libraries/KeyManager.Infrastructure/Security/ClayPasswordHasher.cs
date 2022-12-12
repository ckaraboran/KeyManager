using Microsoft.AspNetCore.Identity;

namespace KeyManager.Infrastructure.Security;

public static class ClayPasswordHasher
{
    private static PasswordHasher<User> _instance;
    private static readonly object Padlock = new();

    private static PasswordHasher<User> Instance
    {
        get
        {
            if (_instance == null)
                lock (Padlock)
                {
                    if (_instance == null) _instance = new PasswordHasher<User>();
                }

            return _instance;
        }
    }

    public static bool IsPasswordOk(User user, string hashedPassword, string password)
    {
        return Instance.VerifyHashedPassword(user, hashedPassword, password) == PasswordVerificationResult.Success;
    }

    public static string HashPassword(User user, string password)
    {
        return Instance.HashPassword(user, password);
    }
}
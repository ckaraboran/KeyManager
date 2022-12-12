namespace KeyManager.Domain.Interfaces;

public interface IPasswordRepository
{
    public Task<bool> CheckPasswordAsync(string password);
}
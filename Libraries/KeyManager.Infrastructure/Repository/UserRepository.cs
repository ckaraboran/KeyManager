namespace KeyManager.Infrastructure.Repository;

public class UserRepository : GenericRepository<User>, IPasswordRepository
{
    public UserRepository(DataContext context) : base(context)
    {
    }

    public async Task<bool> CheckPasswordAsync(string password)
    {
        return await Context.Set<User>().AnyAsync(t => t.Password == password);
    }
}
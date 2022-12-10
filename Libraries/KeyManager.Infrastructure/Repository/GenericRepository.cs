namespace KeyManager.Infrastructure.Repository;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly DataContext _context;

    public GenericRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> expression)
    {
        return await _context.Set<T>().Where(expression).ToListAsync();
    }

    public async Task<T> GetByIdAsync(long id)
    {
        return await _context.Set<T>().FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> expression)
    {
        return await _context.Set<T>().SingleOrDefaultAsync(expression);
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await SaveChangesAsync();

        return entity;
    }


    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);

        await SaveChangesAsync();
    }

    public async Task<T> UpdateAsync(T entity)
    {
        var existingEntity = await _context.Set<T>().FindAsync(entity.Id);

        if (existingEntity != null)
        {
            _context.Entry(existingEntity).State = EntityState.Modified;
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            await SaveChangesAsync();
        }

        return existingEntity!;
    }

    private async Task SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            _context.ChangeTracker.Clear();

            throw;
        }
    }
}
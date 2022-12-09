using Boilerplate.Infrastructure.Extensions;

namespace Boilerplate.Infrastructure;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Dummy> Dummies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                     .Where(x => typeof(ISoftDelete).IsAssignableFrom(x.ClrType)))
            entityType.AddSoftDeleteQueryFilter();

        base.OnModelCreating(modelBuilder);
    }
}
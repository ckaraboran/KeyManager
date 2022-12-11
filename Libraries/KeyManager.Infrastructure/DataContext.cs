using KeyManager.Infrastructure.Extensions;
using KeyManager.Infrastructure.Maps;

namespace KeyManager.Infrastructure;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Dummy> Dummies { get; set; }
    public DbSet<Door> Doors { get; set; }
    public DbSet<Incident> Incidents { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UsersRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EntityMapBase<Dummy>());
        modelBuilder.ApplyConfiguration(new EntityMapBase<Door>());
        modelBuilder.ApplyConfiguration(new EntityMapBase<Incident>());
        modelBuilder.ApplyConfiguration(new EntityMapBase<Permission>());
        modelBuilder.ApplyConfiguration(new EntityMapBase<Role>());
        modelBuilder.ApplyConfiguration(new EntityMapBase<User>());
        modelBuilder.ApplyConfiguration(new EntityMapBase<UserRole>());
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ChangeTracker.SetAuditProperties();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override int SaveChanges()
    {
        ChangeTracker.SetAuditProperties();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        ChangeTracker.SetAuditProperties();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new())
    {
        ChangeTracker.SetAuditProperties();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
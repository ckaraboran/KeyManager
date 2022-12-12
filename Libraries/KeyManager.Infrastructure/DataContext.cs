using System.Diagnostics.CodeAnalysis;
using KeyManager.Domain.Enums;
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
        Seed(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private static void Seed(ModelBuilder modelBuilder)
    {
        AddSeedUserWithPassword(modelBuilder, 1, KnownRoles.OfficeManager.ToString()
            , "AQAAAAEAACcQAAAAEFUGForCcdWHYJyckgcjZ0pFQhrgt4Eqe+6PGIX5ikKvEpA59nqexR8t9vGf9rkqzA==");
        AddSeedUserWithPassword(modelBuilder, 2, KnownRoles.Director.ToString()
            , "AQAAAAEAACcQAAAAEBvydgTOzCeMJb7wl7/t5ocKay40ZlGb1S7aMs2y8TH9nu20KZY/HCnmEN8UlOHbBw==");
        AddSeedUserWithPassword(modelBuilder, 3, KnownRoles.OfficeUser.ToString()
            , "AQAAAAEAACcQAAAAEIy8r8Fw3fH8XbRcZQ4Twu9FAm8smsLBIb1rhUFxZ00XEyfRvxTZtSTV7HGESbz/VA==");
    }

    private static void AddSeedUserWithPassword(ModelBuilder modelBuilder, long id, string name, string password)
    {
        var user = new User
        {
            Id = id, Username = name, Name = name,
            Surname = name,
            Password = password
        };
        var role = new Role { Id = id, Name = name };
        modelBuilder.Entity<Role>().HasData(role);
        modelBuilder.Entity<User>().HasData(user);
        modelBuilder.Entity<UserRole>().HasData(new UserRole { Id = id, UserId = user.Id, RoleId = role.Id });
    }

    [ExcludeFromCodeCoverage]
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ChangeTracker.SetAuditProperties();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    [ExcludeFromCodeCoverage]
    public override int SaveChanges()
    {
        ChangeTracker.SetAuditProperties();
        return base.SaveChanges();
    }

    [ExcludeFromCodeCoverage]
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        ChangeTracker.SetAuditProperties();
        return base.SaveChangesAsync(cancellationToken);
    }

    [ExcludeFromCodeCoverage]
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new())
    {
        ChangeTracker.SetAuditProperties();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
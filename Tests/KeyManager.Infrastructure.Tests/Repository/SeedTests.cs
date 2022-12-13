using KeyManager.Domain.Enums;

namespace KeyManager.Infrastructure.Tests.Repository;

public class SeedTests : IDisposable
{
    private readonly DataContext _dataContext;

    public SeedTests()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _dataContext = new DataContext(optionsBuilder.Options);
        _dataContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool _)
    {
        _dataContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task Given_RoleSeedValues_When_Requested_Then__Returned_In_Query()
    {
        var repository = new GenericRepository<Role>(_dataContext);

        //Act
        var roles = await repository.GetAllAsync();

        //Assert
        Assert.Equal(Enum.GetValues(typeof(KnownRoles)).Length, roles.Count);
        foreach (var knownRole in Enum.GetValues(typeof(KnownRoles)))
            Assert.Contains(roles, r => r.Name == knownRole.ToString());
    }

    [Fact]
    public async Task Given_UserSeedValues_When_Requested_Then__Returned_In_Query()
    {
        var repository = new GenericRepository<User>(_dataContext);

        //Act
        var roles = await repository.GetAllAsync();

        //Assert
        Assert.Equal(Enum.GetValues(typeof(KnownRoles)).Length, roles.Count);
        foreach (var knownRole in Enum.GetValues(typeof(KnownRoles)))
            Assert.Contains(roles, r => r.Name == knownRole.ToString());
    }

    [Fact]
    public async Task Given_UserRoleSeedValues_When_Requested_Then__ReturnSameCountOfValues()
    {
        var repository = new GenericRepository<UserRole>(_dataContext);

        //Act
        var roles = await repository.GetAllAsync();

        //Assert
        Assert.Equal(Enum.GetValues(typeof(KnownRoles)).Length, roles.Count);
    }
}
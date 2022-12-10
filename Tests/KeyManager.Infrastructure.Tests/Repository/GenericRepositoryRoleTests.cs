namespace KeyManager.Infrastructure.Tests.Repository;

public class GenericRepositoryRoleTests : IDisposable
{
    private readonly DataContext _dataContext;

    public GenericRepositoryRoleTests()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _dataContext = new DataContext(optionsBuilder.Options);
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
    public async Task Given_Entities_When_Added_To_Db_Then_Should_Returned_In_Query()
    {
        //Arrange
        var mockRoles = new List<Role>
        {
            new() { Id = 1, Name = "TestName1" },
            new() { Id = 2, Name = "TestName2" },
            new() { Id = 3, Name = "TestName3" }
        };

        _dataContext.AddRange(mockRoles);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Role>(_dataContext);

        //Act
        var roles = await repository.GetAllAsync();

        //Assert
        Assert.Equal(mockRoles.Count, roles.Count);
    }

    [Fact]
    public async Task GenericRepository_GetAsync_ShouldReturnRole()
    {
        //Arrange
        var mockRole = new Role { Id = 1, Name = "TestName1" };
        _dataContext.Roles.Add(mockRole);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Role>(_dataContext);

        //Act
        var role = await repository.GetByIdAsync(mockRole.Id);

        //Assert
        Assert.Equal(mockRole.Id, role.Id);
        Assert.Equal(mockRole.Name, role.Name);
    }

    [Fact]
    public async Task GenericRepository_GetAsync_WithGivenId_ShouldReturnRole()
    {
        //Arrange
        var mockRole = new Role { Id = 1, Name = "TestName1" };
        _dataContext.Roles.Add(mockRole);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Role>(_dataContext);

        //Act
        var roles = await repository.GetAllAsync();

        //Assert
        Assert.Equal(mockRole.Id, roles[0].Id);
        Assert.Equal(mockRole.Name, roles[0].Name);
    }

    [Fact]
    public async Task GenericRepository_GetAsync_WithGivenExpression_ShouldReturnRole()
    {
        //Arrange
        var mockRole = new Role { Id = 1, Name = "TestName1" };
        _dataContext.Roles.Add(mockRole);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Role>(_dataContext);

        //Act
        var role = await repository.GetAsync(e => e.Id == 1);

        //Assert
        Assert.Equal(mockRole.Id, role.Id);
        Assert.Equal(mockRole.Name, role.Name);
    }

    [Fact]
    public async Task GenericRepository_FindAsync_WithGivenExpression_ShouldReturnRole()
    {
        //Arrange
        var mockRole = new Role { Id = 1, Name = "TestName1" };
        _dataContext.Roles.Add(mockRole);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Role>(_dataContext);

        //Act
        var roles = await repository.FindAsync(e => e.Id == 1);

        //Assert
        Assert.Equal(mockRole.Id, roles[0].Id);
        Assert.Equal(mockRole.Name, roles[0].Name);
    }

    [Fact]
    public async Task GenericRepository_AddAsync_WithGivenRole_ShouldReturnRole()
    {
        //Arrange
        var mockRole = new Role { Id = 1, Name = "TestName1" };
        var repository = new GenericRepository<Role>(_dataContext);

        //Act
        var role = await repository.AddAsync(mockRole);

        //Assert
        Assert.Equal(mockRole.Id, role.Id);
        Assert.Equal(mockRole.Name, role.Name);
    }

    [Fact]
    public async Task GenericRepository_DeleteAsync_WithGivenRole_ShouldDeleteRole()
    {
        //Arrange
        var mockRole = new Role { Id = 1, Name = "TestName1" };
        _dataContext.Roles.Add(mockRole);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Role>(_dataContext);

        //Act
        await repository.DeleteAsync(mockRole);
        var role = await repository.GetByIdAsync(mockRole.Id);

        //Assert
        Assert.Null(role);
    }

    [Fact]
    public async Task GenericRepository_UpdateAsync_WithGivenRole_ShouldReturnUpdatedRole()
    {
        //Arrange
        var mockRole = new Role { Id = 1, Name = "TestName1" };
        _dataContext.Roles.Add(mockRole);
        await _dataContext.SaveChangesAsync();
        mockRole.Name = "TestName2";

        var repository = new GenericRepository<Role>(_dataContext);

        //Act
        var role = await repository.UpdateAsync(mockRole);

        //Assert
        Assert.Equal(mockRole.Id, role.Id);
        Assert.Equal(mockRole.Name, role.Name);
    }

    [Fact]
    public async Task GenericRepository_AddAsync_WithGivenEntity_ThrowsDbUpdateException()
    {
        await using var dbConnection = new SqliteConnection("DataSource=:memory:");
        dbConnection.Open();
        var dbContext = CreateDataContext(dbConnection, new MockFailCommandInterceptor());
        var repository = new GenericRepository<Role>(dbContext);

        async Task Result()
        {
            await repository.AddAsync(new Role
            {
                Name = "TestName1"
            });
        }

        await Assert.ThrowsAsync<DbUpdateException>(Result);
    }

    private static DataContext CreateDataContext(DbConnection connection, params IInterceptor[]? interceptors)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>().UseSqlite(connection);

        if (interceptors != null) optionsBuilder.AddInterceptors(interceptors);

        var dbContext = new DataContext(optionsBuilder.Options);
        dbContext.Database.EnsureCreated();

        return dbContext;
    }
}
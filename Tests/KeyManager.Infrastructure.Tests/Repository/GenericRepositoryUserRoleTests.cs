namespace KeyManager.Infrastructure.Tests.Repository;

public class GenericRepositoryUserRoleTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly Role _mockRole = new() { Id = new Random().Next(), Name = "Role1" };

    private readonly User _mockUser = new()
    {
        Id = new Random().Next(),
        Name = "User1", Surname = "User surname1", Username = "Test Username",
        Password = "Test Password"
    };

    public GenericRepositoryUserRoleTests()
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
    public async Task Given_UserRole_When_AddedToDb_Then_ReturnedInQuery()
    {
        //Arrange
        var mockUserRoles = new List<UserRole>
        {
            new() { Id = 1, User = _mockUser, Role = _mockRole },
            new() { Id = 2, User = _mockUser, Role = _mockRole },
            new() { Id = 3, User = _mockUser, Role = _mockRole }
        };

        _dataContext.AddRange(mockUserRoles);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<UserRole>(_dataContext);

        //Act
        var userRoles = await repository.GetAllAsync();

        //Assert
        Assert.Equal(mockUserRoles.Count, userRoles.Count);
    }

    [Fact]
    public async Task Given_UserRole_When_GetAsync_Then_ReturnUserRole()
    {
        //Arrange
        var mockUserRole = new UserRole { Id = 1, User = _mockUser, Role = _mockRole };
        _dataContext.UsersRoles.Add(mockUserRole);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<UserRole>(_dataContext);

        //Act
        var userRole = await repository.GetByIdAsync(mockUserRole.Id);

        //Assert
        Assert.Equal(mockUserRole.Id, userRole.Id);
        Assert.Equal(_mockUser.Id, userRole.UserId);
        Assert.Equal(_mockRole.Id, userRole.RoleId);
    }

    [Fact]
    public async Task Given_UserRole_When_GetAsync_WithGivenId_Then_ReturnUserRole()
    {
        //Arrange
        var mockUserRole = new UserRole { Id = 1, User = _mockUser, Role = _mockRole };
        _dataContext.UsersRoles.Add(mockUserRole);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<UserRole>(_dataContext);

        //Act
        var userRoles = await repository.GetAllAsync();

        //Assert
        Assert.Equal(mockUserRole.Id, userRoles[0].Id);
        Assert.Equal(_mockUser.Id, userRoles[0].UserId);
        Assert.Equal(_mockRole.Id, userRoles[0].RoleId);
    }

    [Fact]
    public async Task Given_UserRole_When_GetAsync_WithGivenExpression_Then_ReturnUserRole()
    {
        //Arrange
        var mockUserRole = new UserRole { Id = 1, User = _mockUser, Role = _mockRole };
        _dataContext.UsersRoles.Add(mockUserRole);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<UserRole>(_dataContext);

        //Act
        var userRole = await repository.GetAsync(e => e.Id == 1);

        //Assert
        Assert.Equal(mockUserRole.Id, userRole.Id);
        Assert.Equal(_mockUser.Id, userRole.UserId);
        Assert.Equal(_mockRole.Id, userRole.RoleId);
    }

    [Fact]
    public async Task Given_UserRole_When_FindAsync_WithGivenExpression_Then_ReturnUserRole()
    {
        //Arrange
        var mockUserRole = new UserRole { Id = 1, User = _mockUser, Role = _mockRole };
        _dataContext.UsersRoles.Add(mockUserRole);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<UserRole>(_dataContext);

        //Act
        var userRoles = await repository.FindAsync(e => e.Id == 1);

        //Assert
        Assert.Equal(mockUserRole.Id, userRoles[0].Id);
        Assert.Equal(mockUserRole.UserId, userRoles[0].UserId);
        Assert.Equal(mockUserRole.RoleId, userRoles[0].RoleId);
    }

    [Fact]
    public async Task Given_UserRole_When_AddAsync_Then_ReturnUserRole()
    {
        //Arrange
        var mockUserRole = new UserRole { Id = 1, User = _mockUser, Role = _mockRole };
        var repository = new GenericRepository<UserRole>(_dataContext);

        //Act
        var userRole = await repository.AddAsync(mockUserRole);

        //Assert
        Assert.Equal(mockUserRole.Id, userRole.Id);
        Assert.Equal(_mockUser.Id, userRole.UserId);
        Assert.Equal(_mockRole.Id, userRole.RoleId);
    }

    [Fact]
    public async Task Given_UserRole_When_DeleteAsync_Then_DeleteUserRole()
    {
        //Arrange
        var mockUserRole = new UserRole { Id = 1, User = _mockUser, Role = _mockRole };
        _dataContext.UsersRoles.Add(mockUserRole);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<UserRole>(_dataContext);

        //Act
        await repository.DeleteAsync(mockUserRole);
        var userRole = await repository.GetByIdAsync(mockUserRole.Id);

        //Assert
        Assert.Null(userRole);
    }

    [Fact]
    public async Task Given_UserRole_When_UpdateAsync_WithGivenUser_Then_ReturnUpdatedUserRole()
    {
        //Arrange
        var mockUserRole = new UserRole { Id = 1, User = _mockUser, Role = _mockRole };
        _dataContext.UsersRoles.Add(mockUserRole);
        await _dataContext.SaveChangesAsync();
        mockUserRole.UserId = 2;

        var repository = new GenericRepository<UserRole>(_dataContext);

        //Act
        var userRole = await repository.UpdateAsync(mockUserRole);

        //Assert
        Assert.Equal(mockUserRole.Id, userRole.Id);
        Assert.Equal(mockUserRole.UserId, userRole.UserId);
        Assert.Equal(_mockRole.Id, userRole.RoleId);
    }

    [Fact]
    public async Task Given_UserRole_When_UpdateAsync_WithGivenRole_Then_ReturnUpdatedUserRole()
    {
        //Arrange
        var mockUserRole = new UserRole { Id = 1, User = _mockUser, Role = _mockRole };
        _dataContext.UsersRoles.Add(mockUserRole);
        await _dataContext.SaveChangesAsync();
        mockUserRole.RoleId = 2;

        var repository = new GenericRepository<UserRole>(_dataContext);

        //Act
        var userRole = await repository.UpdateAsync(mockUserRole);

        //Assert
        Assert.Equal(mockUserRole.Id, userRole.Id);
        Assert.Equal(_mockUser.Id, userRole.UserId);
        Assert.Equal(mockUserRole.RoleId, userRole.RoleId);
    }

    [Fact]
    public async Task Given_UserRole_When_AddAsync_Then_ThrowsDbUpdateException()
    {
        await using var dbConnection = new SqliteConnection("DataSource=:memory:");
        dbConnection.Open();
        var dbContext = CreateDataContext(dbConnection, new MockFailCommandInterceptor());
        var repository = new GenericRepository<UserRole>(dbContext);

        async Task Result()
        {
            await repository.AddAsync(new UserRole
            {
                Id = 1, User = _mockUser, Role = _mockRole
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
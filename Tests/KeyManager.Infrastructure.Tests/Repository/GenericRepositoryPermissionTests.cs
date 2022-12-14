namespace KeyManager.Infrastructure.Tests.Repository;

public class GenericRepositoryPermissionTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly Door _mockDoor = new() { Id = new Random().Next(), Name = "Door1" };

    private readonly User _mockUser = new()
    {
        Id = new Random().Next(),
        Name = "User1", Surname = "User surname1", Username = "Test Username",
        Password = "Test Password"
    };

    public GenericRepositoryPermissionTests()
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
    public async Task Given_Permission_WhenAddedToDb_Then_Returned_In_Query()
    {
        //Arrange
        var mockPermissions = new List<Permission>
        {
            new() { Id = 1, User = _mockUser, Door = _mockDoor },
            new() { Id = 2, User = _mockUser, Door = _mockDoor },
            new() { Id = 3, User = _mockUser, Door = _mockDoor }
        };

        _dataContext.AddRange(mockPermissions);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Permission>(_dataContext);

        //Act
        var permissions = await repository.GetAllAsync();

        //Assert
        Assert.Equal(mockPermissions.Count, permissions.Count);
    }

    [Fact]
    public async Task Given_Permission_When_GetAsync_ThenReturnPermission()
    {
        //Arrange
        var mockPermission = new Permission { Id = 1, User = _mockUser, Door = _mockDoor };
        _dataContext.Permissions.Add(mockPermission);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Permission>(_dataContext);

        //Act
        var permission = await repository.GetByIdAsync(mockPermission.Id);

        //Assert
        Assert.Equal(mockPermission.Id, permission.Id);
        Assert.Equal(_mockUser.Id, permission.UserId);
        Assert.Equal(_mockDoor.Id, permission.DoorId);
    }

    [Fact]
    public async Task Given_Permission_When_GetAsync_WithGivenId_ThenReturnPermission()
    {
        //Arrange
        var mockPermission = new Permission { Id = 1, User = _mockUser, Door = _mockDoor };
        _dataContext.Permissions.Add(mockPermission);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Permission>(_dataContext);

        //Act
        var permissions = await repository.GetAllAsync();

        //Assert
        Assert.Equal(mockPermission.Id, permissions[0].Id);
        Assert.Equal(_mockUser.Id, permissions[0].UserId);
        Assert.Equal(_mockDoor.Id, permissions[0].DoorId);
    }

    [Fact]
    public async Task Given_Permission_When_GetAsync_WithGivenExpression_ThenReturnPermission()
    {
        //Arrange
        var mockPermission = new Permission { Id = 1, User = _mockUser, Door = _mockDoor };
        _dataContext.Permissions.Add(mockPermission);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Permission>(_dataContext);

        //Act
        var permission = await repository.GetAsync(e => e.Id == 1);

        //Assert
        Assert.Equal(mockPermission.Id, permission.Id);
        Assert.Equal(_mockUser.Id, permission.UserId);
        Assert.Equal(_mockDoor.Id, permission.DoorId);
    }

    [Fact]
    public async Task Given_Permission_When_FindAsync_WithGivenExpression_ThenReturnPermission()
    {
        //Arrange
        var mockPermission = new Permission { Id = 1, User = _mockUser, Door = _mockDoor };
        _dataContext.Permissions.Add(mockPermission);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Permission>(_dataContext);

        //Act
        var permissions = await repository.FindAsync(e => e.Id == 1);

        //Assert
        Assert.Equal(mockPermission.Id, permissions[0].Id);
        Assert.Equal(mockPermission.UserId, permissions[0].UserId);
        Assert.Equal(mockPermission.DoorId, permissions[0].DoorId);
    }

    [Fact]
    public async Task Given_Permission_When_AddAsync_ThenReturnPermission()
    {
        //Arrange
        var mockPermission = new Permission { Id = 1, User = _mockUser, Door = _mockDoor };
        var repository = new GenericRepository<Permission>(_dataContext);

        //Act
        var permission = await repository.AddAsync(mockPermission);

        //Assert
        Assert.Equal(mockPermission.Id, permission.Id);
        Assert.Equal(_mockUser.Id, permission.UserId);
        Assert.Equal(_mockDoor.Id, permission.DoorId);
    }

    [Fact]
    public async Task Given_Permission_When_DeleteAsync_ThenDeletePermission()
    {
        //Arrange
        var mockPermission = new Permission { Id = 1, User = _mockUser, Door = _mockDoor };
        _dataContext.Permissions.Add(mockPermission);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Permission>(_dataContext);

        //Act
        await repository.DeleteAsync(mockPermission);
        var permission = await repository.GetByIdAsync(mockPermission.Id);

        //Assert
        Assert.Null(permission);
    }

    [Fact]
    public async Task Given_Permission_When_UpdateUser_ThenReturnUpdatedPermission()
    {
        //Arrange
        var mockPermission = new Permission { Id = 1, User = _mockUser, Door = _mockDoor };
        _dataContext.Permissions.Add(mockPermission);
        await _dataContext.SaveChangesAsync();
        mockPermission.UserId = 2;

        var repository = new GenericRepository<Permission>(_dataContext);

        //Act
        var permission = await repository.UpdateAsync(mockPermission);

        //Assert
        Assert.Equal(mockPermission.Id, permission.Id);
        Assert.Equal(mockPermission.UserId, permission.UserId);
        Assert.Equal(_mockDoor.Id, permission.DoorId);
    }

    [Fact]
    public async Task Given_Permission_When_UpdateDoor_ThenReturnUpdatedPermission()
    {
        //Arrange
        var mockPermission = new Permission { Id = 1, User = _mockUser, Door = _mockDoor };
        _dataContext.Permissions.Add(mockPermission);
        await _dataContext.SaveChangesAsync();
        mockPermission.DoorId = 2;

        var repository = new GenericRepository<Permission>(_dataContext);

        //Act
        var permission = await repository.UpdateAsync(mockPermission);

        //Assert
        Assert.Equal(mockPermission.Id, permission.Id);
        Assert.Equal(_mockUser.Id, permission.UserId);
        Assert.Equal(mockPermission.DoorId, permission.DoorId);
    }

    [Fact]
    public async Task Given_Permission_When_DbError_Then_ThrowsDbUpdateException()
    {
        await using var dbConnection = new SqliteConnection("DataSource=:memory:");
        dbConnection.Open();
        var dbContext = CreateDataContext(dbConnection, new MockFailCommandInterceptor());
        var repository = new GenericRepository<Permission>(dbContext);

        async Task Result()
        {
            await repository.AddAsync(new Permission
            {
                Id = 1, User = _mockUser, Door = _mockDoor
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
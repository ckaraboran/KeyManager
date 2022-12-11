namespace KeyManager.Infrastructure.Tests.Repository;

public class GenericRepositoryUserTests : IDisposable
{
    private readonly DataContext _dataContext;

    public GenericRepositoryUserTests()
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
        var mockUsers = new List<User>
        {
            new() { Id = 1, Username = "Test Username", Name = "TestName1", Surname = "TestSurname1" },
            new() { Id = 2, Username = "Test Username2", Name = "TestName2", Surname = "TestSurname2" },
            new() { Id = 3, Username = "Test Username3", Name = "TestName3", Surname = "TestSurname3" }
        };

        _dataContext.AddRange(mockUsers);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<User>(_dataContext);

        //Act
        var users = await repository.GetAllAsync();

        //Assert
        Assert.Equal(mockUsers.Count, users.Count);
    }

    [Fact]
    public async Task GenericRepository_GetAsync_ShouldReturnUser()
    {
        //Arrange
        var mockUser = new User
        {
            Id = 1, Username = "Test Username", Name = "TestName1", Surname = "TestSurname1"
        };
        _dataContext.Users.Add(mockUser);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<User>(_dataContext);

        //Act
        var user = await repository.GetByIdAsync(mockUser.Id);

        //Assert
        Assert.Equal(mockUser.Id, user.Id);
        Assert.Equal(mockUser.Name, user.Name);
        Assert.Equal(mockUser.Surname, user.Surname);
        Assert.Equal(mockUser.Username, user.Username);
        Assert.False(user.IsDeleted);
    }

    [Fact]
    public async Task GenericRepository_GetAsync_WithGivenId_ShouldReturnUser()
    {
        //Arrange
        var mockUser = new User
        {
            Id = 1, Username = "Test Username", Name = "TestName1", Surname = "TestSurname1"
        };
        _dataContext.Users.Add(mockUser);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<User>(_dataContext);

        //Act
        var users = await repository.GetAllAsync();

        //Assert
        Assert.Equal(mockUser.Id, users[0].Id);
        Assert.Equal(mockUser.Name, users[0].Name);
        Assert.Equal(mockUser.Surname, users[0].Surname);
        Assert.Equal(mockUser.Username, users[0].Username);
        Assert.False(users[0].IsDeleted);
    }

    [Fact]
    public async Task GenericRepository_GetAsync_WithGivenExpression_ShouldReturnUser()
    {
        //Arrange
        var mockUser = new User
        {
            Id = 1, Username = "Test Username", Name = "TestName1", Surname = "TestSurname1"
        };
        _dataContext.Users.Add(mockUser);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<User>(_dataContext);

        //Act
        var user = await repository.GetAsync(e => e.Id == 1);

        //Assert
        Assert.Equal(mockUser.Id, user.Id);
        Assert.Equal(1, user.Id);
        Assert.Equal(mockUser.Name, user.Name);
        Assert.Equal(mockUser.Surname, user.Surname);
        Assert.Equal(mockUser.Username, user.Username);
        Assert.False(user.IsDeleted);
    }

    [Fact]
    public async Task GenericRepository_FindAsync_WithGivenExpression_ShouldReturnUser()
    {
        //Arrange
        var mockUser = new User
        {
            Id = 1, Username = "Test Username", Name = "TestName1", Surname = "TestSurname1"
        };
        _dataContext.Users.Add(mockUser);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<User>(_dataContext);

        //Act
        var users = await repository.FindAsync(e => e.Id == 1);

        //Assert
        Assert.Equal(mockUser.Id, users[0].Id);
        Assert.Equal(mockUser.Name, users[0].Name);
        Assert.Equal(mockUser.Surname, users[0].Surname);
        Assert.Equal(mockUser.Username, users[0].Username);
        Assert.False(users[0].IsDeleted);
    }

    [Fact]
    public async Task GenericRepository_AddAsync_WithGivenUser_ShouldReturnUser()
    {
        //Arrange
        var mockUser = new User
        {
            Id = 1, Username = "Test Username", Name = "TestName1", Surname = "TestSurname1"
        };
        var repository = new GenericRepository<User>(_dataContext);

        //Act
        var user = await repository.AddAsync(mockUser);

        //Assert
        Assert.Equal(mockUser.Id, user.Id);
        Assert.Equal(mockUser.Name, user.Name);
        Assert.Equal(mockUser.Surname, user.Surname);
        Assert.Equal(mockUser.Username, user.Username);
        Assert.False(user.IsDeleted);
    }

    [Fact]
    public async Task GenericRepository_DeleteAsync_WithGivenUser_ShouldDeleteUser()
    {
        //Arrange
        var mockUser = new User
        {
            Id = 1, Username = "Test Username", Name = "TestName1", Surname = "TestSurname1"
        };
        _dataContext.Users.Add(mockUser);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<User>(_dataContext);

        //Act
        await repository.DeleteAsync(mockUser);
        var user = await repository.GetByIdAsync(mockUser.Id);

        //Assert
        Assert.Null(user);
    }

    [Fact]
    public async Task GenericRepository_UpdateAsync_WithGivenUser_ShouldReturnUpdatedUser()
    {
        //Arrange
        var mockUser = new User
        {
            Id = 1, Username = "Test Username", Name = "TestName1", Surname = "TestSurname1"
        };
        _dataContext.Users.Add(mockUser);
        await _dataContext.SaveChangesAsync();
        mockUser.Name = "TestName2";

        var repository = new GenericRepository<User>(_dataContext);

        //Act
        var user = await repository.UpdateAsync(mockUser);

        //Assert
        Assert.Equal(mockUser.Id, user.Id);
        Assert.Equal(mockUser.Name, user.Name);
        Assert.Equal(mockUser.Surname, user.Surname);
        Assert.Equal(mockUser.Username, user.Username);
        Assert.False(user.IsDeleted);
    }

    [Fact]
    public async Task GenericRepository_AddAsync_WithGivenEntity_ThrowsDbUpdateException()
    {
        await using var dbConnection = new SqliteConnection("DataSource=:memory:");
        dbConnection.Open();
        var dbContext = CreateDataContext(dbConnection, new MockFailCommandInterceptor());
        var repository = new GenericRepository<User>(dbContext);

        async Task Result()
        {
            await repository.AddAsync(new User
            {
                Id = 1, Username = "Test Username", Name = "TestName1", Surname = "TestSurname1"
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
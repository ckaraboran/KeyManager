namespace KeyManager.Infrastructure.Tests.Repository;

public class GenericRepositoryDoorTests: IDisposable
{
    private readonly DataContext _dataContext;

    public GenericRepositoryDoorTests()
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
        var mockDoors = new List<Door>
        {
            new() { Id = 1, Name = "TestName1" },
            new() { Id = 2, Name = "TestName2" },
            new() { Id = 3, Name = "TestName3" }
        };

        _dataContext.AddRange(mockDoors);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Door>(_dataContext);

        //Act
        var doors = await repository.GetAllAsync();

        //Assert
        Assert.Equal(mockDoors.Count, doors.Count);
    }

    [Fact]
    public async Task GenericRepository_GetAsync_ShouldReturnDoor()
    {
        //Arrange
        var mockDoor = new Door { Id = 1, Name = "TestName1" };
        _dataContext.Doors.Add(mockDoor);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Door>(_dataContext);

        //Act
        var door = await repository.GetAllAsync();

        //Assert
        Assert.Equal(mockDoor.Id, door[0].Id);
        Assert.Equal(mockDoor.Name, door[0].Name);
    }

    [Fact]
    public async Task GenericRepository_GetAsync_WithGivenId_ShouldReturnDoor()
    {
        //Arrange
        var mockDoor = new Door { Id = 1, Name = "TestName1" };
        _dataContext.Doors.Add(mockDoor);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Door>(_dataContext);

        //Act
        var door = await repository.GetAsync(mockDoor.Id);

        //Assert
        Assert.Equal(mockDoor.Id, door.Id);
        Assert.Equal(mockDoor.Name, door.Name);
    }

    [Fact]
    public async Task GenericRepository_GetAsync_WithGivenExpression_ShouldReturnDoor()
    {
        //Arrange
        var mockDoor = new Door { Id = 1, Name = "TestName1" };
        _dataContext.Doors.Add(mockDoor);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Door>(_dataContext);

        //Act
        var door = await repository.GetAsync(e => e.Id == 1);

        //Assert
        Assert.Equal(mockDoor.Id, door.Id);
        Assert.Equal(mockDoor.Name, door.Name);
    }

    [Fact]
    public async Task GenericRepository_FindAsync_WithGivenExpression_ShouldReturnDoor()
    {
        //Arrange
        var mockDoor = new Door { Id = 1, Name = "TestName1" };
        _dataContext.Doors.Add(mockDoor);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Door>(_dataContext);

        //Act
        var users = await repository.FindAsync(e => e.Id == 1);

        //Assert
        Assert.Equal(mockDoor.Id, users[0].Id);
        Assert.Equal(mockDoor.Name, users[0].Name);
    }

    [Fact]
    public async Task GenericRepository_AddAsync_WithGivenDoor_ShouldReturnDoor()
    {
        //Arrange
        var mockDoor = new Door { Id = 1, Name = "TestName1" };
        var repository = new GenericRepository<Door>(_dataContext);

        //Act
        var door = await repository.AddAsync(mockDoor);

        //Assert
        Assert.Equal(mockDoor.Id, door.Id);
        Assert.Equal(mockDoor.Name, door.Name);
    }

    [Fact]
    public async Task GenericRepository_DeleteAsync_WithGivenDoor_ShouldDeleteDoor()
    {
        //Arrange
        var mockDoor = new Door { Id = 1, Name = "TestName1" };
        _dataContext.Doors.Add(mockDoor);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Door>(_dataContext);

        //Act
        await repository.DeleteAsync(mockDoor);
        var door = await repository.GetAsync(mockDoor.Id);

        //Assert
        Assert.Null(door);
    }

    [Fact]
    public async Task GenericRepository_UpdateAsync_WithGivenDoor_ShouldReturnUpdatedDoor()
    {
        //Arrange
        var mockDoor = new Door { Id = 1, Name = "TestName1" };
        _dataContext.Doors.Add(mockDoor);
        await _dataContext.SaveChangesAsync();
        mockDoor.Name = "TestName2";

        var repository = new GenericRepository<Door>(_dataContext);

        //Act
        var door = await repository.UpdateAsync(mockDoor);

        //Assert
        Assert.Equal(mockDoor.Id, door.Id);
        Assert.Equal(mockDoor.Name, door.Name);
    }

    [Fact]
    public async Task GenericRepository_AddAsync_WithGivenEntity_ThrowsDbUpdateException()
    {
        await using var dbConnection = new SqliteConnection("DataSource=:memory:");
        dbConnection.Open();
        var dbContext = CreateDataContext(dbConnection, new MockFailCommandInterceptor());
        var repository = new GenericRepository<Door>(dbContext);

        async Task Result()
        {
            await repository.AddAsync(new Door
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
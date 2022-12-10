namespace KeyManager.Infrastructure.Tests.Repository;

public class GenericRepositoryTests : IDisposable
{
    private readonly DataContext _dataContext;

    public GenericRepositoryTests()
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
        var mockDummies = new List<Dummy>
        {
            new() { Id = 1, Name = "TestName1" },
            new() { Id = 2, Name = "TestName2" },
            new() { Id = 3, Name = "TestName3" }
        };
        
        _dataContext.AddRange(mockDummies);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Dummy>(_dataContext);

        //Act
        var dummies = await repository.GetAllAsync();

        //Assert
        Assert.Equal(mockDummies.Count, dummies.Count);
    }

    [Fact]
    public async Task GenericRepository_GetAsync_ShouldReturnDummy()
    {
        //Arrange
        var mockDummy = new Dummy { Id = 1, Name = "TestName1" };
        _dataContext.Dummies.Add(mockDummy);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Dummy>(_dataContext);

        //Act
        var dummy = await repository.GetAsync(mockDummy.Id);

        //Assert
        Assert.Equal(mockDummy.Id, dummy.Id);
        Assert.Equal(mockDummy.Name, dummy.Name);
    }

    [Fact]
    public async Task GenericRepository_GetAsync_WithGivenId_ShouldReturnDummy()
    {
        //Arrange
        var mockDummy = new Dummy { Id = 1, Name = "TestName1" };
        _dataContext.Dummies.Add(mockDummy);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Dummy>(_dataContext);

        //Act
        var employee = await repository.GetAsync(mockDummy.Id);

        //Assert
        Assert.Equal(mockDummy.Id, employee.Id);
        Assert.Equal(mockDummy.Name, employee.Name);
    }

    [Fact]
    public async Task GenericRepository_GetAsync_WithGivenExpression_ShouldReturnDummy()
    {
        //Arrange
        var mockDummy = new Dummy { Id = 1, Name = "TestName1" };
        _dataContext.Dummies.Add(mockDummy);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Dummy>(_dataContext);

        //Act
        var employee = await repository.GetAsync(e => e.Id == 1);

        //Assert
        Assert.Equal(mockDummy.Id, employee.Id);
        Assert.Equal(mockDummy.Name, employee.Name);
    }

    [Fact]
    public async Task GenericRepository_FindAsync_WithGivenExpression_ShouldReturnDummy()
    {
        //Arrange
        var mockDummy = new Dummy { Id = 1, Name = "TestName1" };
        _dataContext.Dummies.Add(mockDummy);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Dummy>(_dataContext);

        //Act
        var employees = await repository.FindAsync(e => e.Id == 1);

        //Assert
        Assert.Equal(mockDummy.Id, employees[0].Id);
        Assert.Equal(mockDummy.Name, employees[0].Name);
    }

    [Fact]
    public async Task GenericRepository_AddAsync_WithGivenDummy_ShouldReturnDummy()
    {
        //Arrange
        var mockDummy = new Dummy { Id = 1, Name = "TestName1" };
        var repository = new GenericRepository<Dummy>(_dataContext);

        //Act
        var employee = await repository.AddAsync(mockDummy);

        //Assert
        Assert.Equal(mockDummy.Id, employee.Id);
        Assert.Equal(mockDummy.Name, employee.Name);
    }

    [Fact]
    public async Task GenericRepository_DeleteAsync_WithGivenDummy_ShouldDeleteDummy()
    {
        //Arrange
        var mockDummy = new Dummy { Id = 1, Name = "TestName1" };
        _dataContext.Dummies.Add(mockDummy);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Dummy>(_dataContext);

        //Act
        await repository.DeleteAsync(mockDummy);
        var employee = await repository.GetAsync(mockDummy.Id);

        //Assert
        Assert.Null(employee);
    }

    [Fact]
    public async Task GenericRepository_UpdateAsync_WithGivenDummy_ShouldReturnUpdatedDummy()
    {
        //Arrange
        var mockDummy = new Dummy { Id = 1, Name = "TestName1" };
        _dataContext.Dummies.Add(mockDummy);
        await _dataContext.SaveChangesAsync();
        mockDummy.Name = "TestName2";

        var repository = new GenericRepository<Dummy>(_dataContext);

        //Act
        var employee = await repository.UpdateAsync(mockDummy);

        //Assert
        Assert.Equal(mockDummy.Id, employee.Id);
        Assert.Equal(mockDummy.Name, employee.Name);
    }

    [Fact]
    public async Task GenericRepository_AddAsync_WithGivenEntity_ThrowsDbUpdateException()
    {
        await using var dbConnection = new SqliteConnection("DataSource=:memory:");
        dbConnection.Open();
        var dbContext = CreateDataContext(dbConnection, new MockFailCommandInterceptor());

        var repository = new GenericRepository<Dummy>(dbContext);

        async Task Result()
        {
            await repository.AddAsync(new Dummy
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
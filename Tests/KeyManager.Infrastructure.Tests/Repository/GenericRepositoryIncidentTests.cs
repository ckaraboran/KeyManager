namespace KeyManager.Infrastructure.Tests.Repository;

public class GenericRepositoryIncidentTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly Door _mockDoor = new() { Id = new Random().Next(), Name = "Door1" };
    private readonly User _mockUser = new() { Id = new Random().Next(), Name = "Name1", Surname = "Surname1" };

    public GenericRepositoryIncidentTests()
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
        var mockIncidents = new List<Incident>
        {
            new() { Id = 1, User = _mockUser, Door = _mockDoor, EventDate = DateTime.Now },
            new() { Id = 2, User = _mockUser, Door = _mockDoor, EventDate = DateTime.Now },
            new() { Id = 3, User = _mockUser, Door = _mockDoor, EventDate = DateTime.Now }
        };

        _dataContext.AddRange(mockIncidents);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Incident>(_dataContext);

        //Act
        var incidents = await repository.GetAllAsync();

        //Assert
        Assert.Equal(mockIncidents.Count, incidents.Count);
    }

    [Fact]
    public async Task GenericRepository_GetAsync_ShouldReturnIncident()
    {
        //Arrange
        var mockIncident = new Incident { Id = 1, User = _mockUser, Door = _mockDoor, EventDate = DateTime.Now };
        _dataContext.Incidents.Add(mockIncident);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Incident>(_dataContext);

        //Act
        var incident = await repository.GetAsync(mockIncident.Id);

        //Assert
        Assert.Equal(mockIncident.Id, incident.Id);
        Assert.Equal(_mockUser.Id, incident.UserId);
        Assert.Equal(_mockDoor.Id, incident.DoorId);
        Assert.Equal(mockIncident.EventDate, incident.EventDate);
    }

    [Fact]
    public async Task GenericRepository_GetAsync_WithGivenId_ShouldReturnIncident()
    {
        //Arrange
        var mockIncident = new Incident { Id = 1, User = _mockUser, Door = _mockDoor, EventDate = DateTime.Now };
        _dataContext.Incidents.Add(mockIncident);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Incident>(_dataContext);

        //Act
        var incidents = await repository.GetAllAsync();

        //Assert
        Assert.Equal(mockIncident.Id, incidents[0].Id);
        Assert.Equal(_mockUser.Id, incidents[0].UserId);
        Assert.Equal(_mockDoor.Id, incidents[0].DoorId);
        Assert.Equal(mockIncident.EventDate, incidents[0].EventDate);
    }

    [Fact]
    public async Task GenericRepository_GetAsync_WithGivenExpression_ShouldReturnIncident()
    {
        //Arrange
        var mockIncident = new Incident { Id = 1, User = _mockUser, Door = _mockDoor, EventDate = DateTime.Now };
        _dataContext.Incidents.Add(mockIncident);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Incident>(_dataContext);

        //Act
        var incident = await repository.GetAsync(e => e.Id == 1);

        //Assert
        Assert.Equal(mockIncident.Id, incident.Id);
        Assert.Equal(_mockUser.Id, incident.UserId);
        Assert.Equal(_mockDoor.Id, incident.DoorId);
        Assert.Equal(mockIncident.EventDate, incident.EventDate);
    }

    [Fact]
    public async Task GenericRepository_FindAsync_WithGivenExpression_ShouldReturnIncident()
    {
        //Arrange
        var mockIncident = new Incident { Id = 1, User = _mockUser, Door = _mockDoor, EventDate = DateTime.Now };
        _dataContext.Incidents.Add(mockIncident);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Incident>(_dataContext);

        //Act
        var incidents = await repository.FindAsync(e => e.Id == 1);

        //Assert
        Assert.Equal(mockIncident.Id, incidents[0].Id);
        Assert.Equal(_mockUser.Id, incidents[0].UserId);
        Assert.Equal(_mockDoor.Id, incidents[0].DoorId);
        Assert.Equal(mockIncident.EventDate, incidents[0].EventDate);
    }

    [Fact]
    public async Task GenericRepository_AddAsync_WithGivenIncident_ShouldReturnIncident()
    {
        //Arrange
        var mockIncident = new Incident { Id = 1, User = _mockUser, Door = _mockDoor, EventDate = DateTime.Now };
        var repository = new GenericRepository<Incident>(_dataContext);

        //Act
        var incident = await repository.AddAsync(mockIncident);

        //Assert
        Assert.Equal(mockIncident.Id, incident.Id);
        Assert.Equal(_mockUser.Id, incident.UserId);
        Assert.Equal(_mockDoor.Id, incident.DoorId);
        Assert.Equal(mockIncident.EventDate, incident.EventDate);
    }

    [Fact]
    public async Task GenericRepository_DeleteAsync_WithGivenIncident_ShouldDeleteIncident()
    {
        //Arrange
        var mockIncident = new Incident { Id = 1, User = _mockUser, Door = _mockDoor, EventDate = DateTime.Now };
        _dataContext.Incidents.Add(mockIncident);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Incident>(_dataContext);

        //Act
        await repository.DeleteAsync(mockIncident);
        var incident = await repository.GetAsync(mockIncident.Id);

        //Assert
        Assert.Null(incident);
    }

    [Fact]
    public async Task GenericRepository_UpdateAsync_WithGivenIncidentUser_ShouldReturnUpdatedIncident()
    {
        //Arrange
        var mockIncident = new Incident { Id = 1, User = _mockUser, Door = _mockDoor, EventDate = DateTime.Now };
        _dataContext.Incidents.Add(mockIncident);
        await _dataContext.SaveChangesAsync();
        mockIncident.UserId = 2;

        var repository = new GenericRepository<Incident>(_dataContext);

        //Act
        var incident = await repository.UpdateAsync(mockIncident);

        //Assert
        Assert.Equal(mockIncident.Id, incident.Id);
        Assert.Equal(mockIncident.UserId, incident.UserId);
        Assert.Equal(_mockDoor.Id, incident.DoorId);
        Assert.Equal(mockIncident.EventDate, incident.EventDate);
    }

    [Fact]
    public async Task GenericRepository_UpdateAsync_WithGivenIncidentDoor_ShouldReturnUpdatedIncident()
    {
        //Arrange
        var mockIncident = new Incident { Id = 1, User = _mockUser, Door = _mockDoor, EventDate = DateTime.Now };
        _dataContext.Incidents.Add(mockIncident);
        await _dataContext.SaveChangesAsync();
        mockIncident.DoorId = 2;

        var repository = new GenericRepository<Incident>(_dataContext);

        //Act
        var incident = await repository.UpdateAsync(mockIncident);

        //Assert
        Assert.Equal(mockIncident.Id, incident.Id);
        Assert.Equal(_mockUser.Id, incident.UserId);
        Assert.Equal(mockIncident.DoorId, incident.DoorId);
        Assert.Equal(mockIncident.EventDate, incident.EventDate);
    }

    [Fact]
    public async Task GenericRepository_AddAsync_WithGivenEntity_ThrowsDbUpdateException()
    {
        await using var dbConnection = new SqliteConnection("DataSource=:memory:");
        dbConnection.Open();
        var dbContext = CreateDataContext(dbConnection, new MockFailCommandInterceptor());
        var repository = new GenericRepository<Incident>(dbContext);

        async Task Result()
        {
            await repository.AddAsync(new Incident
            {
                Id = 1, User = _mockUser, Door = _mockDoor, EventDate = DateTime.Now
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
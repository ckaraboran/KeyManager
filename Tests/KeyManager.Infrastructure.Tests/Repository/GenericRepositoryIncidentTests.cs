namespace KeyManager.Infrastructure.Tests.Repository;

public class GenericRepositoryIncidentTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly Door _mockDoor = new() { Id = new Random().Next(), Name = "Door1" };

    private readonly User _mockUser = new()
    {
        Id = new Random().Next(), Name = "Name1",
        Surname = "Surname1", Password = "Password1"
    };

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
    public async Task Given_Incident_When_AddedToDb_Then_ReturnedInQuery()
    {
        //Arrange
        var mockIncidents = new List<Incident>
        {
            new() { Id = 1, User = _mockUser, Door = _mockDoor, IncidentDate = DateTimeOffset.UtcNow },
            new() { Id = 2, User = _mockUser, Door = _mockDoor, IncidentDate = DateTimeOffset.UtcNow },
            new() { Id = 3, User = _mockUser, Door = _mockDoor, IncidentDate = DateTimeOffset.UtcNow }
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
    public async Task Given_Incident_When_GetAsync_Then_ReturnIncident()
    {
        //Arrange
        var mockIncident = new Incident
            { Id = 1, User = _mockUser, Door = _mockDoor, IncidentDate = DateTimeOffset.UtcNow };
        _dataContext.Incidents.Add(mockIncident);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Incident>(_dataContext);

        //Act
        var incident = await repository.GetByIdAsync(mockIncident.Id);

        //Assert
        Assert.Equal(mockIncident.Id, incident.Id);
        Assert.Equal(_mockUser.Id, incident.UserId);
        Assert.Equal(_mockDoor.Id, incident.DoorId);
        Assert.Equal(mockIncident.IncidentDate, incident.IncidentDate);
    }

    [Fact]
    public async Task Given_Incident_When_GetAsync_With_GivenId_Then_ReturnIncident()
    {
        //Arrange
        var mockIncident = new Incident
            { Id = 1, User = _mockUser, Door = _mockDoor, IncidentDate = DateTimeOffset.UtcNow };
        _dataContext.Incidents.Add(mockIncident);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Incident>(_dataContext);

        //Act
        var incidents = await repository.GetAllAsync();

        //Assert
        Assert.Equal(mockIncident.Id, incidents[0].Id);
        Assert.Equal(_mockUser.Id, incidents[0].UserId);
        Assert.Equal(_mockDoor.Id, incidents[0].DoorId);
        Assert.Equal(mockIncident.IncidentDate, incidents[0].IncidentDate);
    }

    [Fact]
    public async Task Given_Incident_When_GetAsync_With_GivenExpression_Then_ReturnIncident()
    {
        //Arrange
        var mockIncident = new Incident
            { Id = 1, User = _mockUser, Door = _mockDoor, IncidentDate = DateTimeOffset.UtcNow };
        _dataContext.Incidents.Add(mockIncident);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Incident>(_dataContext);

        //Act
        var incident = await repository.GetAsync(e => e.Id == 1);

        //Assert
        Assert.Equal(mockIncident.Id, incident.Id);
        Assert.Equal(_mockUser.Id, incident.UserId);
        Assert.Equal(_mockDoor.Id, incident.DoorId);
        Assert.Equal(mockIncident.IncidentDate, incident.IncidentDate);
    }

    [Fact]
    public async Task Given_Incident_When_FindAsync_With_GivenExpression_Then_ReturnIncident()
    {
        //Arrange
        var mockIncident = new Incident
            { Id = 1, User = _mockUser, Door = _mockDoor, IncidentDate = DateTimeOffset.UtcNow };
        _dataContext.Incidents.Add(mockIncident);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Incident>(_dataContext);

        //Act
        var incidents = await repository.FindAsync(e => e.Id == 1);

        //Assert
        Assert.Equal(mockIncident.Id, incidents[0].Id);
        Assert.Equal(_mockUser.Id, incidents[0].UserId);
        Assert.Equal(_mockDoor.Id, incidents[0].DoorId);
        Assert.Equal(mockIncident.IncidentDate, incidents[0].IncidentDate);
    }

    [Fact]
    public async Task Given_Incident_When_AddAsync_Then_ReturnIncident()
    {
        //Arrange
        var mockIncident = new Incident
            { Id = 1, User = _mockUser, Door = _mockDoor, IncidentDate = DateTimeOffset.UtcNow };
        var repository = new GenericRepository<Incident>(_dataContext);

        //Act
        var incident = await repository.AddAsync(mockIncident);

        //Assert
        Assert.Equal(mockIncident.Id, incident.Id);
        Assert.Equal(_mockUser.Id, incident.UserId);
        Assert.Equal(_mockDoor.Id, incident.DoorId);
        Assert.Equal(mockIncident.IncidentDate, incident.IncidentDate);
    }

    [Fact]
    public async Task Given_Incident_When_DeleteAsync_Then_DeleteIncident()
    {
        //Arrange
        var mockIncident = new Incident
            { Id = 1, User = _mockUser, Door = _mockDoor, IncidentDate = DateTimeOffset.UtcNow };
        _dataContext.Incidents.Add(mockIncident);
        await _dataContext.SaveChangesAsync();

        var repository = new GenericRepository<Incident>(_dataContext);

        //Act
        await repository.DeleteAsync(mockIncident);
        var incident = await repository.GetByIdAsync(mockIncident.Id);

        //Assert
        Assert.Null(incident);
    }

    [Fact]
    public async Task Given_Incident_When_UpdateUserAsync_Then_ReturnUpdatedIncident()
    {
        //Arrange
        var mockIncident = new Incident
            { Id = 1, User = _mockUser, Door = _mockDoor, IncidentDate = DateTimeOffset.UtcNow };
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
        Assert.Equal(mockIncident.IncidentDate, incident.IncidentDate);
    }

    [Fact]
    public async Task Given_Incident_When_UpdateDoorAsync_Then_ReturnUpdatedIncident()
    {
        //Arrange
        var mockIncident = new Incident
            { Id = 1, User = _mockUser, Door = _mockDoor, IncidentDate = DateTimeOffset.UtcNow };
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
        Assert.Equal(mockIncident.IncidentDate, incident.IncidentDate);
    }

    [Fact]
    public async Task Given_Incident_When_DatabaseError_Then_ThrowsDbUpdateException()
    {
        await using var dbConnection = new SqliteConnection("DataSource=:memory:");
        dbConnection.Open();
        var dbContext = CreateDataContext(dbConnection, new MockFailCommandInterceptor());
        var repository = new GenericRepository<Incident>(dbContext);

        async Task Result()
        {
            await repository.AddAsync(new Incident
            {
                Id = 1, User = _mockUser, Door = _mockDoor, IncidentDate = DateTimeOffset.UtcNow
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
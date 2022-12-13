using System;
using KeyManager.Application.Queries.Incidents;
using KeyManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Tests.Queries.Incidents;

public class GetIncidentsQueryHandlerTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly GetIncidentsQueryHandler _incidentHandler;

    public GetIncidentsQueryHandlerTests()
    {
        var dbOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dataContext = new DataContext(dbOptions);
        _incidentHandler = new GetIncidentsQueryHandler(_dataContext);
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
    public async Task Given_Incident_When_GetAsync_ShouldReturnIncidentsDto()
    {
        //Arrange

        var mockUser = new User
        {
            Id = 1001,
            Name = "Test User1",
            Surname = "Test Surname1",
            Password = "Test Password1"
        };
        var mockDoor = new Door
        {
            Id = 1001,
            Name = "Test Door1"
        };
        var mockIncidents = new List<Incident>
        {
            new() { Id = 1001, UserId = mockUser.Id, DoorId = mockDoor.Id },
            new() { Id = 1002, UserId = mockUser.Id, DoorId = mockDoor.Id }
        };
        await _dataContext.AddAsync(mockDoor);
        await _dataContext.AddAsync(mockUser);
        await _dataContext.AddRangeAsync(mockIncidents);
        await _dataContext.SaveChangesAsync();
        //Act
        var result = await _incidentHandler.Handle(new GetIncidentsQuery(), default);

        //Assert
        Assert.Equal(mockIncidents[0].Id, result[0].Id);
        Assert.Equal(mockUser.Name, result[0].UserName);
        Assert.Equal(mockDoor.Name, result[0].DoorName);
        Assert.Equal(mockIncidents[1].Id, result[1].Id);
        Assert.Equal(mockUser.Name, result[1].UserName);
        Assert.Equal(mockDoor.Name, result[1].DoorName);
    }
}
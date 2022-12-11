using System;
using KeyManager.Application.Queries.Doors;
using KeyManager.Domain.Entities;
using KeyManager.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Tests.Queries.Doors;

public class GetDoorByIdQueryHandlerTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly GetDoorByIdQueryHandler _doorHandler;

    public GetDoorByIdQueryHandlerTests()
    {
        var dbOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dataContext = new DataContext(dbOptions);
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _doorHandler = new GetDoorByIdQueryHandler(_dataContext, mapper);
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
    public async Task Door_GetAsync_WithGivenId_ShouldReturnDoorDto()
    {
        //Arrange

        var mockDoors = new List<Door>
        {
            new() { Id = new Random().Next(), Name = "Door1" },
            new() { Id = new Random().Next(), Name = "Door2" },
            new() { Id = new Random().Next(), Name = "Door3" }
        };
        var newMockDoor = new Door
        {
            Id = new Random().Next(),
            Name = "Door4"
        };
        mockDoors.Add(newMockDoor);

        await _dataContext.AddRangeAsync(mockDoors);
        await _dataContext.SaveChangesAsync();

        //Act
        var result = await _doorHandler.Handle(new GetDoorByIdQuery(newMockDoor.Id), default);

        //Assert
        Assert.Equal(result.Id, newMockDoor.Id);
        Assert.Equal(result.Name, newMockDoor.Name);
    }
}
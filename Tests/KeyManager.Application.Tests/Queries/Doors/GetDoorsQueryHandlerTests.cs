using System;
using KeyManager.Application.Queries.Doors;
using KeyManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Tests.Queries.Doors;

public class GetDoorsQueryHandlerTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly GetDoorsQueryHandler _doorHandler;

    public GetDoorsQueryHandlerTests()
    {
        var dbOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dataContext = new DataContext(dbOptions);
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _doorHandler = new GetDoorsQueryHandler(_dataContext, mapper);
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
    public async Task Door_GetAsync_ShouldReturnDoorsDto()
    {
        //Arrange
        var mockDoors = new List<Door>
        {
            new() { Id = 1, Name = "Test" },
            new() { Id = 2, Name = "Test2" }
        };
        await _dataContext.AddRangeAsync(mockDoors);
        await _dataContext.SaveChangesAsync();
        //Act
        var result = await _doorHandler.Handle(new GetDoorsQuery(), default);

        //Assert
        Assert.Equal(result[0].Id, mockDoors[0].Id);
        Assert.Equal(result[0].Name, mockDoors[0].Name);
        Assert.Equal(result[1].Id, mockDoors[1].Id);
        Assert.Equal(result[1].Name, mockDoors[1].Name);
    }
}
using System;
using KeyManager.Application.Queries;
using KeyManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Tests.Queries;

public class GetAllDummiesQueryHandlerTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly GetAllDummiesQueryHandler _dummyHandler;

    public GetAllDummiesQueryHandlerTests()
    {
        var dbOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dataContext = new DataContext(dbOptions);
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _dummyHandler = new GetAllDummiesQueryHandler(_dataContext, mapper);
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
    public async Task Dummy_GetAsync_ShouldReturnAllDummiesDto()
    {
        //Arrange
        var mockDummies = new List<Dummy>
        {
            new() { Id = 1001, Name = "Test" },
            new() { Id = 1002, Name = "Test2" }
        };
        var mockDummiesDto = new List<DummyDto>
        {
            new() { Id = 1001, Name = "Test" },
            new() { Id = 1002, Name = "Test2" }
        };
        await _dataContext.AddRangeAsync(mockDummies);
        await _dataContext.SaveChangesAsync();
        //Act
        var result = await _dummyHandler.Handle(new GetAllDummiesQuery(), default);

        //Assert
        Assert.Equal(result[0].Id, mockDummiesDto[0].Id);
        Assert.Equal(result[0].Name, mockDummiesDto[0].Name);
        Assert.Equal(result[1].Id, mockDummiesDto[1].Id);
        Assert.Equal(result[1].Name, mockDummiesDto[1].Name);
    }
}
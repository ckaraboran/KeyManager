using System;
using KeyManager.Application.Mappings;
using KeyManager.Application.Queries;
using KeyManager.Domain.Entities;
using KeyManager.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Tests.Queries;

public class GetDummyQueryHandlerTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly GetDummyQueryHandler _dummyHandler;

    public GetDummyQueryHandlerTests()
    {
        var dbOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dataContext = new DataContext(dbOptions);
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _dummyHandler = new GetDummyQueryHandler(_dataContext, mapper);
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
    public async Task Dummy_GetAsync_WithGivenId_ShouldReturnDummyDto()
    {
        //Arrange
        var mockDummy = new Dummy
        {
            Id = 1,
            Name = "Test"
        };
        var mockDummyDto = new DummyDto
        {
            Id = 1,
            Name = "Test"
        };
        await _dataContext.AddAsync(mockDummy);
        await _dataContext.SaveChangesAsync();

        //Act
        var result = await _dummyHandler.Handle(new GetDummyQuery(1), default);

        //Assert
        Assert.Equal(result.Id, mockDummyDto.Id);
        Assert.Equal(result.Name, mockDummyDto.Name);
    }
}
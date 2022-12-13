using System;
using KeyManager.Application.Queries.Roles;
using KeyManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Tests.Queries.Roles;

public class GetRolesQueryHandlerTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly GetRolesQueryHandler _roleHandler;

    public GetRolesQueryHandlerTests()
    {
        var dbOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dataContext = new DataContext(dbOptions);
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _roleHandler = new GetRolesQueryHandler(_dataContext, mapper);
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
    public async Task Given_Role_When_GetAsync_ShouldReturnRolesDto()
    {
        //Arrange
        var mockRoles = new List<Role>
        {
            new() { Id = 1001, Name = "Test" },
            new() { Id = 1002, Name = "Test2" }
        };
        await _dataContext.AddRangeAsync(mockRoles);
        await _dataContext.SaveChangesAsync();
        //Act
        var result = await _roleHandler.Handle(new GetRolesQuery(), default);

        //Assert
        Assert.Equal(mockRoles[0].Id, result[0].Id);
        Assert.Equal(mockRoles[0].Name, result[0].Name);
        Assert.Equal(mockRoles[1].Id, result[1].Id);
        Assert.Equal(mockRoles[1].Name, result[1].Name);
    }
}
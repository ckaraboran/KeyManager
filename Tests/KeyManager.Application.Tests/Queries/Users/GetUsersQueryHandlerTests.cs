using System;
using KeyManager.Application.Queries.Users;
using KeyManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Tests.Queries.Users;

public class GetUsersQueryHandlerTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly GetUsersQueryHandler _userHandler;

    public GetUsersQueryHandlerTests()
    {
        var dbOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dataContext = new DataContext(dbOptions);
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _userHandler = new GetUsersQueryHandler(_dataContext, mapper);
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
    public async Task User_GetAsync_ShouldReturnUsersDto()
    {
        //Arrange
        var mockUsers = new List<User>
        {
            new()
            {
                Id = 1, Name = "Test Name1", Surname = "Test Surname1", Username = "Test Username1",
                Password = "Test Password1"
            },
            new()
            {
                Id = 2, Name = "Test Name2", Surname = "Test Surname2", Username = "Test Username2",
                Password = "Test Password2"
            }
        };
        await _dataContext.AddRangeAsync(mockUsers);
        await _dataContext.SaveChangesAsync();
        //Act
        var result = await _userHandler.Handle(new GetUsersQuery(), default);

        //Assert
        Assert.Equal(result[0].Id, mockUsers[0].Id);
        Assert.Equal(result[0].Name, mockUsers[0].Name);
        Assert.Equal(result[0].Surname, mockUsers[0].Surname);
        Assert.Equal(result[1].Id, mockUsers[1].Id);
        Assert.Equal(result[1].Name, mockUsers[1].Name);
        Assert.Equal(result[1].Surname, mockUsers[1].Surname);
    }
}
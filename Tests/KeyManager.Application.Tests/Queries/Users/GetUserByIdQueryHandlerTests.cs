using System;
using KeyManager.Application.Queries.Users;
using KeyManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Tests.Queries.Users;

public class GetUserByIdQueryHandlerTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly GetUserByIdQueryHandler _userHandler;

    public GetUserByIdQueryHandlerTests()
    {
        var dbOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dataContext = new DataContext(dbOptions);
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _userHandler = new GetUserByIdQueryHandler(_dataContext, mapper);
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
    public async Task User_GetAsync_WithGivenId_ShouldReturnUserDto()
    {
        //Arrange

        var mockUsers = new List<User>
        {
            new()
            {
                Id = new Random().Next(), Name = "User Name 1", Surname = "User Surname 1", Username = "Username 1",
                Password = "Password 1"
            },
            new()
            {
                Id = new Random().Next(), Name = "User Name 2", Surname = "User Surname 2", Username = "Username 2",
                Password = "Password 2"
            },
            new()
            {
                Id = new Random().Next(), Name = "User Name 3", Surname = "User Surname 3", Username = "Username 3",
                Password = "Password 3"
            }
        };
        var newMockUser = new User
        {
            Id = new Random().Next(),
            Name = "User Name 4",
            Surname = "User Surname 4",
            Username = "Username 4",
            Password = "Password 4"
        };
        mockUsers.Add(newMockUser);

        await _dataContext.AddRangeAsync(mockUsers);
        await _dataContext.SaveChangesAsync();

        //Act
        var result = await _userHandler.Handle(new GetUserByIdQuery(newMockUser.Id), default);

        //Assert
        Assert.Equal(result.Id, newMockUser.Id);
        Assert.Equal(result.Name, newMockUser.Name);
        Assert.Equal(result.Surname, newMockUser.Surname);
        Assert.Equal(result.Username, newMockUser.Username);
    }

    [Fact]
    public async Task User_GetAsync_WithNotExistUsername_ShouldThrowNotFoundException()
    {
        //Arrange

        var mockUsers = new List<User>
        {
            new()
            {
                Id = 1, Name = "User Name 1", Surname = "User Surname 1", Username = "Username 1",
                Password = "Password 1"
            },
            new()
            {
                Id = 2, Name = "User Name 2", Surname = "User Surname 2", Username = "Username 2",
                Password = "Password 2"
            },
            new()
            {
                Id = 3, Name = "User Name 3", Surname = "User Surname 3", Username = "Username 3",
                Password = "Password 3"
            }
        };

        await _dataContext.AddRangeAsync(mockUsers);
        await _dataContext.SaveChangesAsync();

        //Act
        Task Result()
        {
            return _userHandler.Handle(new GetUserByIdQuery(4), default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<RecordNotFoundException>(Result);
        Assert.Equal("User not found. User ID: '4'", exception.Message);
    }
}
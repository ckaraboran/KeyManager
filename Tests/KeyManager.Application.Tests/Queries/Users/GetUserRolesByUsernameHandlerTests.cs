using System;
using KeyManager.Application.Queries.Users;
using KeyManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Tests.Queries.Users;

public class GetUserRolesByUsernameHandlerTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly GetUserRolesByUsernameHandler _userHandler;

    public GetUserRolesByUsernameHandlerTests()
    {
        var dbOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dataContext = new DataContext(dbOptions);
        _userHandler = new GetUserRolesByUsernameHandler(_dataContext);
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
                Password = "Password 1"
            },
            new()
            {
                Id = new Random().Next(), Name = "User Name 3", Surname = "User Surname 3", Username = "Username 3",
                Password = "Password 1"
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
        var newMockRole = new Role
        {
            Id = new Random().Next(),
            Name = "User Role 1"
        };
        var newMockAnotherRole = new Role
        {
            Id = new Random().Next(),
            Name = "User Role 2"
        };
        var newMockUserRole = new UserRole
        {
            Id = new Random().Next(),
            UserId = newMockUser.Id,
            RoleId = newMockRole.Id
        };
        var newMockAnotherUserRole = new UserRole
        {
            Id = new Random().Next(),
            UserId = newMockUser.Id,
            RoleId = newMockAnotherRole.Id
        };
        mockUsers.Add(newMockUser);

        await _dataContext.AddRangeAsync(mockUsers);
        await _dataContext.AddAsync(newMockRole);
        await _dataContext.AddAsync(newMockAnotherRole);
        await _dataContext.AddAsync(newMockUserRole);
        await _dataContext.AddAsync(newMockAnotherUserRole);
        await _dataContext.SaveChangesAsync();

        //Act
        var result = await _userHandler.Handle(new GetUserRolesByUsername(newMockUser.Username), default);

        //Assert
        Assert.Equal(result.Username, newMockUser.Username);
        Assert.Equal(result.RoleNames[0], newMockRole.Name);
        Assert.Equal(result.RoleNames[1], newMockAnotherRole.Name);
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
            return _userHandler.Handle(new GetUserRolesByUsername("NotExistUsername"), default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<RecordNotFoundException>(Result);
        Assert.Equal("User not found. Username: 'NotExistUsername'", exception.Message);
    }
}
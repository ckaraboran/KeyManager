using System;
using KeyManager.Application.Queries.UserRoles;
using KeyManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Tests.Queries.UserRoles;

public class GetUserRolesQueryHandlerTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly GetUserRolesQueryHandler _userRoleHandler;

    public GetUserRolesQueryHandlerTests()
    {
        var dbOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _dataContext = new DataContext(dbOptions);
        _userRoleHandler = new GetUserRolesQueryHandler(_dataContext);
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
    public async Task UserRole_GetAsync_ShouldReturnUserRolesDto()
    {
        //Arrange

        var mockUser = new User
        {
            Id = 1001,
            Name = "Test User1",
            Surname = "Test Surname1",
            Password = "Test Password1"
        };
        var mockRole = new Role
        {
            Id = 1001,
            Name = "Test Role1"
        };
        var mockUserRoles = new List<UserRole>
        {
            new() { Id = 1001, UserId = mockUser.Id, RoleId = mockRole.Id },
            new() { Id = 1002, UserId = mockUser.Id, RoleId = mockRole.Id }
        };
        await _dataContext.AddAsync(mockRole);
        await _dataContext.AddAsync(mockUser);
        await _dataContext.AddRangeAsync(mockUserRoles);
        await _dataContext.SaveChangesAsync();
        //Act
        var result = await _userRoleHandler.Handle(new GetUserRolesQuery(), default);

        //Assert
        Assert.Equal(mockUserRoles[0].Id, result[0].Id);
        Assert.Equal(mockUser.Name, result[0].UserName);
        Assert.Equal(mockRole.Name, result[0].RoleName);
        Assert.Equal(mockUserRoles[1].Id, result[1].Id);
        Assert.Equal(mockUser.Name, result[1].UserName);
        Assert.Equal(mockRole.Name, result[1].RoleName);
    }
}
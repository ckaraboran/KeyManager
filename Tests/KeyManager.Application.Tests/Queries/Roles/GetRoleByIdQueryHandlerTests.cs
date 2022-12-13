using System;
using KeyManager.Application.Queries.Roles;
using KeyManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Tests.Queries.Roles;

public class GetRoleByIdQueryHandlerTests : IDisposable
{
    private readonly DataContext _dataContext;
    private readonly GetRoleByIdQueryHandler _roleHandler;

    public GetRoleByIdQueryHandlerTests()
    {
        var dbOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _dataContext = new DataContext(dbOptions);
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _roleHandler = new GetRoleByIdQueryHandler(_dataContext, mapper);
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
    public async Task Given_RoleGet_When_WithGivenId_Then_ReturnRoleDto()
    {
        //Arrange

        var mockRoles = new List<Role>
        {
            new() { Id = new Random().Next(), Name = "Role1" },
            new() { Id = new Random().Next(), Name = "Role2" },
            new() { Id = new Random().Next(), Name = "Role3" }
        };
        var newMockRole = new Role
        {
            Id = new Random().Next(),
            Name = "Role4"
        };
        mockRoles.Add(newMockRole);

        await _dataContext.AddRangeAsync(mockRoles);
        await _dataContext.SaveChangesAsync();

        //Act
        var result = await _roleHandler.Handle(new GetRoleByIdQuery(newMockRole.Id), default);

        //Assert
        Assert.Equal(result.Id, newMockRole.Id);
        Assert.Equal(result.Name, newMockRole.Name);
    }
}
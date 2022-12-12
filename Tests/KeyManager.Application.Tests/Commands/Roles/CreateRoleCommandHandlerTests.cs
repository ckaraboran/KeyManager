using KeyManager.Application.Commands.Roles;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands.Roles;

public class CreateRoleCommandHandlerTests
{
    private readonly Mock<IGenericRepository<Role>> _mockRoleRepository;
    private readonly CreateRoleCommandHandler _roleHandler;

    public CreateRoleCommandHandlerTests()
    {
        _mockRoleRepository = new Mock<IGenericRepository<Role>>();
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _roleHandler = new CreateRoleCommandHandler(_mockRoleRepository.Object, mapper);
    }

    [Fact]
    public async Task Role_Create_WithGivenCreateRoleCommand_ShouldReturnCreateRoleDto()
    {
        //Arrange
        var mockRole = new Role
        {
            Id = 1,
            Name = "Test"
        };
        _mockRoleRepository.Setup(s => s.AddAsync(It.IsAny<Role>())).ReturnsAsync(mockRole);

        //Act
        var result = await _roleHandler.Handle(new CreateRoleCommand(mockRole.Name), default);

        //Assert
        Assert.Equal(result.Id, mockRole.Id);
        Assert.Equal(result.Name, mockRole.Name);

        _mockRoleRepository.VerifyAll();
    }

    [Fact]
    public async Task Role_PostAsync_WithGivenCreateRoleRequest_ShouldThrowExistingRecordException_IfRecordExists()
    {
        //Arrange
        var mockCreateRoleCommand = new CreateRoleCommand("Test");
        var mockRole = new Role
        {
            Id = 1,
            Name = "Test"
        };
        _mockRoleRepository.Setup(s => s.GetAsync(p => p.Name == mockCreateRoleCommand.Name)).ReturnsAsync(mockRole);

        //Act
        Task Result()
        {
            return _roleHandler.Handle(mockCreateRoleCommand, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<RecordAlreadyExistsException>(Result);
        Assert.Equal("There is a role with the same name: 'Test'", exception.Message);
        _mockRoleRepository.VerifyAll();
    }
}
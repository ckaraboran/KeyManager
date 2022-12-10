using KeyManager.Application.Commands;
using KeyManager.Application.Mappings;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands;

public class CreateDummyCommandHandlerTests
{
    private readonly CreateDummyCommandHandler _dummyHandler;
    private readonly Mock<IGenericRepository<Dummy>> _mockDummyRepository;

    public CreateDummyCommandHandlerTests()
    {
        _mockDummyRepository = new Mock<IGenericRepository<Dummy>>();
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _dummyHandler = new CreateDummyCommandHandler(_mockDummyRepository.Object, mapper);
    }

    [Fact]
    public async Task Dummy_Create_WithGivenCreateDummyCommand_ShouldReturnCreateDummyDto()
    {
        //Arrange
        var mockDummyDto = new DummyDto
        {
            Id = 1,
            Name = "Test"
        };
        var mockDummy = new Dummy
        {
            Id = 1,
            Name = "Test"
        };
        _mockDummyRepository.Setup(s => s.AddAsync(It.IsAny<Dummy>())).ReturnsAsync(mockDummy);

        //Act
        var result = await _dummyHandler.Handle(new CreateDummyCommand("Test"), default);

        //Assert
        Assert.Equal(result.Id, mockDummyDto.Id);
        Assert.Equal(result.Name, mockDummyDto.Name);

        _mockDummyRepository.VerifyAll();
    }

    [Fact]
    public async Task Dummy_PostAsync_WithGivenCreateDummyRequest_ShouldThrowExistingRecordException_IfRecordExists()
    {
        //Arrange
        var mockCreateDummyCommand = new CreateDummyCommand("Test");
        var mockDummy = new Dummy
        {
            Id = 1,
            Name = "Test"
        };
        _mockDummyRepository.Setup(s => s.GetAsync(p => p.Name == mockCreateDummyCommand.Name)).ReturnsAsync(mockDummy);

        //Act
        Task Result()
        {
            return _dummyHandler.Handle(mockCreateDummyCommand, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<DummyException>(Result);
        Assert.Equal("There is a dummy. Name: 'Test'", exception.Message);
        _mockDummyRepository.VerifyAll();
    }
}
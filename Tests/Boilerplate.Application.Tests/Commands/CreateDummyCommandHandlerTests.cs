using Boilerplate.Application.Commands;

namespace Boilerplate.Application.Tests.Commands;

public class CreateDummyCommandHandlerTests
{
    private readonly CreateDummyCommandHandler _dummyHandler;
    private readonly Mock<IGenericRepository<Domain.Entities.Dummy>> _mockDummyRepository;
    private readonly Mock<IMapper> _mockMapper;

    public CreateDummyCommandHandlerTests()
    {
        _mockDummyRepository = new Mock<IGenericRepository<Domain.Entities.Dummy>>();
        _mockMapper = new Mock<IMapper>();
        _dummyHandler = new CreateDummyCommandHandler(_mockDummyRepository.Object, _mockMapper.Object);
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
        var mockDummy = new Domain.Entities.Dummy
        {
            Id = 1,
            Name = "Test"
        };
        _mockDummyRepository.Setup(s => s.AddAsync(It.IsAny<Domain.Entities.Dummy>())).ReturnsAsync(mockDummy);
        _mockMapper.Setup(m => m.Map<DummyDto>(It.IsAny<Domain.Entities.Dummy>())).Returns(mockDummyDto);

        //Act
        var result = await _dummyHandler.Handle(new CreateDummyCommand("Test"), default);

        //Assert
        Assert.Equal(result, mockDummyDto);
        _mockDummyRepository.VerifyAll();
    }

    [Fact]
    public async Task Dummy_PostAsync_WithGivenCreateDummyRequest_ShouldThrowExistingRecordException_IfRecordExists()
    {
        //Arrange
        var mockCreateDummyCommand = new CreateDummyCommand("Test");
        var mockDummy = new Domain.Entities.Dummy
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
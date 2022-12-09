using Boilerplate.Application.Commands;

namespace Boilerplate.Application.Tests.Commands;

public class UpdateDummyCommandHandlerTests
{
    private readonly UpdateDummyCommandHandler _dummyHandler;
    private readonly Mock<IGenericRepository<Domain.Entities.Dummy>> _mockDummyRepository;
    private readonly Mock<IMapper> _mockMapper;

    public UpdateDummyCommandHandlerTests()
    {
        _mockDummyRepository = new Mock<IGenericRepository<Domain.Entities.Dummy>>();
        _mockMapper = new Mock<IMapper>();
        _dummyHandler = new UpdateDummyCommandHandler(_mockDummyRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Dummy_PutAsync_WithGivenUpdateDummyRequest_ShouldReturnCreateDummyDto()
    {
        var dummyName = "Test";
        //Arrange
        var mockDummyUpdateCommand = new UpdateDummyCommand(1, dummyName);

        var mockDummyDto = new DummyDto
        {
            Id = 1,
            Name = dummyName
        };
        var mockDummy = new Domain.Entities.Dummy
        {
            Id = 1,
            Name = dummyName
        };
        _mockDummyRepository.Setup(s => s.GetAsync(It.IsAny<int>())).ReturnsAsync(mockDummy);
        _mockDummyRepository.Setup(s => s.UpdateAsync(It.IsAny<Domain.Entities.Dummy>())).ReturnsAsync(mockDummy);
        _mockMapper.Setup(m => m.Map<DummyDto>(It.IsAny<Domain.Entities.Dummy>())).Returns(mockDummyDto);

        //Act
        var result = await _dummyHandler.Handle(mockDummyUpdateCommand, default);

        //Assert
        Assert.Equal(result, mockDummyDto);
        _mockDummyRepository.VerifyAll();
    }

    [Fact]
    public async Task
        Dummy_PutAsync_WithGivenUpdateDummyRequest_ShouldThrowRecordNotFoundException_IfRecordDoesNotExist()
    {
        //Arrange
        var mockUpdateDummyCommand = new UpdateDummyCommand(1, "Test");
        _mockDummyRepository.Setup(s => s.GetAsync(mockUpdateDummyCommand.Id))
            .ReturnsAsync((Domain.Entities.Dummy)null);

        //Act
        Task Result()
        {
            return _dummyHandler.Handle(mockUpdateDummyCommand, default);
        }

        //Assert
        var exception = await Assert.ThrowsAsync<DummyException>(Result);
        Assert.Equal("Dummy is not found while updating. DummyId: '1'", exception.Message);
        _mockDummyRepository.VerifyAll();
    }
}
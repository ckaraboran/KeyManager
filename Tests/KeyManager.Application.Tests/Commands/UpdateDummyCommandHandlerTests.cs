using KeyManager.Application.Commands;
using KeyManager.Application.Mappings;
using KeyManager.Domain.Entities;

namespace KeyManager.Application.Tests.Commands;

public class UpdateDummyCommandHandlerTests
{
    private readonly UpdateDummyCommandHandler _dummyHandler;
    private readonly Mock<IGenericRepository<Dummy>> _mockDummyRepository;

    public UpdateDummyCommandHandlerTests()
    {
        _mockDummyRepository = new Mock<IGenericRepository<Dummy>>();
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _dummyHandler = new UpdateDummyCommandHandler(_mockDummyRepository.Object, mapper);
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
        var mockDummy = new Dummy
        {
            Id = 1,
            Name = dummyName
        };
        _mockDummyRepository.Setup(s => s.GetAsync(It.IsAny<long>())).ReturnsAsync(mockDummy);
        _mockDummyRepository.Setup(s => s.UpdateAsync(It.IsAny<Dummy>())).ReturnsAsync(mockDummy);

        //Act
        var result = await _dummyHandler.Handle(mockDummyUpdateCommand, default);

        //Assert
        Assert.Equal(result.Id, mockDummyDto.Id);
        Assert.Equal(result.Name, mockDummyDto.Name);
        _mockDummyRepository.VerifyAll();
    }

    [Fact]
    public async Task
        Dummy_PutAsync_WithGivenUpdateDummyRequest_ShouldThrowRecordNotFoundException_IfRecordDoesNotExist()
    {
        //Arrange
        var mockUpdateDummyCommand = new UpdateDummyCommand(1, "Test");
        _mockDummyRepository.Setup(s => s.GetAsync(mockUpdateDummyCommand.Id))
            .ReturnsAsync((Dummy)null);

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
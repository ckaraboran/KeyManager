using KeyManager.Application.Queries;
using KeyManager.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KeyManager.Application.Tests.Queries;

public class GetDummyQueryHandlerTests
{
    private readonly GetDummyQueryHandler _dummyHandler;
    private readonly DataContext _dataContext;
    private readonly Mock<IMapper> _mockMapper;

    public GetDummyQueryHandlerTests()
    {
        var dbOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(nameof(GetDummyQueryHandlerTests))
            .Options;
        _dataContext = new DataContext(dbOptions);
        _mockMapper = new Mock<IMapper>();
        _dummyHandler = new GetDummyQueryHandler(_dataContext, _mockMapper.Object);
    }

    [Fact]
    public async Task Dummy_GetAsync_WithGivenId_ShouldReturnDummyDto()
    {
        //Arrange
        var mockDummy = new KeyManager.Domain.Entities.Dummy
        {
            Id = 1,
            Name = "Test"
        };
        var mockDummyDto = new DummyDto
        {
            Id = 1,
            Name = "Test"
        };
        _dataContext.Add(mockDummy);
        _mockMapper.Setup(m => m.Map<DummyDto>(It.IsAny<KeyManager.Domain.Entities.Dummy>())).Returns(mockDummyDto);

        //Act
        var result = await _dummyHandler.Handle(new GetDummyQuery(1), default);

        //Assert
        Assert.Equal(result, mockDummyDto);
    }
}
using Boilerplate.Application.Queries;
using Boilerplate.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Application.Tests.Queries;

public class GetAllDummiesQueryHandlerTests
{
    private readonly GetAllDummiesQueryHandler _dummyHandler;
    private readonly DataContext _dataContext;
    private readonly Mock<IMapper> _mockMapper;

    public GetAllDummiesQueryHandlerTests()
    {
        var dbOptions = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(nameof(GetDummyQueryHandlerTests))
            .Options;

        _dataContext = new DataContext(dbOptions);
        _mockMapper = new Mock<IMapper>();
        _dummyHandler = new GetAllDummiesQueryHandler(_dataContext, _mockMapper.Object);
    }

    [Fact]
    public async Task Dummy_GetAsync_ShouldReturnAllDummiesDto()
    {
        //Arrange
        var mockDummies = new List<Domain.Entities.Dummy>
        {
            new() { Id = 1, Name = "Test" },
            new() { Id = 2, Name = "Test2" }
        };
        var mockDummiesDto = new List<DummyDto>
        {
            new() { Id = 1, Name = "Test" },
            new() { Id = 2, Name = "Test2" }
        };
        _dataContext.AddRange(mockDummies);
        _mockMapper.Setup(m => m.Map<List<DummyDto>>(It.IsAny<List<Domain.Entities.Dummy>>())).Returns(mockDummiesDto);

        //Act
        var result = await _dummyHandler.Handle(new GetAllDummiesQuery(), default);

        //Assert
        Assert.Equal(result, mockDummiesDto);
    }
}
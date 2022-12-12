using System.Threading;
using KeyManager.Api.DTOs.Responses.Incident;
using KeyManager.Application.Queries.Incidents;
using MediatR;

namespace KeyManager.Api.Tests.Controllers;

public class IncidentControllerTests
{
    private readonly Mock<ISender> _mockMediator;
    private readonly IncidentController _sut;

    public IncidentControllerTests()
    {
        _mockMediator = new Mock<ISender>();
        var myProfile = new AutoMapperProfile();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
        var mapper = new Mapper(configuration);
        _sut = new IncidentController(_mockMediator.Object, mapper);
    }

    [Fact]
    public async Task Incident_GetAsync_ShouldReturnAllIncidents()
    {
        //Arrange
        var mockIncidentDto = new List<IncidentWithNamesDto>
        {
            new()
            {
                Id = 1, UserId = 1, UserName = "Username 1", DoorId = 1, DoorName = "Door name 1",
                IncidentDate = DateTimeOffset.Now
            },
            new()
            {
                Id = 1, UserId = 2, UserName = "Username 2", DoorId = 2, DoorName = "Door name 2",
                IncidentDate = DateTimeOffset.Now
            }
        };
        _mockMediator.Setup(s => s.Send(It.IsAny<GetIncidentsQuery>(), It.Is<CancellationToken>(x => x == default)))
            .ReturnsAsync(mockIncidentDto);

        //Act
        var result = await _sut.GetAsync();

        //Assert
        var resultObject = (List<GetIncidentResponse>)Assert.IsType<OkObjectResult>(result.Result).Value;
        Assert.NotNull(resultObject);
        Assert.Equal(mockIncidentDto[0].Id, resultObject[0].Id);
        Assert.Equal(mockIncidentDto[0].UserId, resultObject[0].UserId);
        Assert.Equal(mockIncidentDto[0].UserName, resultObject[0].UserName);
        Assert.Equal(mockIncidentDto[0].DoorId, resultObject[0].DoorId);
        Assert.Equal(mockIncidentDto[0].DoorName, resultObject[0].DoorName);
        Assert.Equal(mockIncidentDto[0].IncidentDate, resultObject[0].IncidentDate);
        Assert.Equal(mockIncidentDto[1].Id, resultObject[1].Id);
        Assert.Equal(mockIncidentDto[1].UserId, resultObject[1].UserId);
        Assert.Equal(mockIncidentDto[1].UserName, resultObject[1].UserName);
        Assert.Equal(mockIncidentDto[1].DoorId, resultObject[1].DoorId);
        Assert.Equal(mockIncidentDto[1].DoorName, resultObject[1].DoorName);
        Assert.Equal(mockIncidentDto[1].IncidentDate, resultObject[1].IncidentDate);
        _mockMediator.VerifyAll();
    }
}
using System;
using System.Threading;
using Boilerplate.Application.Behaviors;
using Boilerplate.Application.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Application.Tests.Behaviors;

public class LoggingBehaviorTests
{
    //properties
    private readonly Mock<ILogger<LoggingBehavior<TestRequest, TestResponse>>> _logger;
    private readonly LoggingBehavior<TestRequest, TestResponse> _loggingBehavior;

    public LoggingBehaviorTests()
    {
        _logger = new Mock<ILogger<LoggingBehavior<TestRequest, TestResponse>>>();
        _loggingBehavior = new LoggingBehavior<TestRequest, TestResponse>(_logger.Object);
    }

    [Fact]
    public async Task Given_Request_When_SentToMediator_Then_Will_Be_Logged()
    {
        //Arrange
        var request = new TestRequest
        {
            Name = "Test"
        };
        var response = new TestResponse
        {
            Name = "Test"
        };
        var expectedLogBeforeMethod = string.Format("----- Handling request {0} ({1})",
            request.GetGenericTypeName(), request);
        var expectedLogAfterMethod = string.Format("----- Request {0} handled - response: {1}",
            request.GetGenericTypeName(), response);

        var requestHandlerDelegate = new Mock<RequestHandlerDelegate<TestResponse>>();
        requestHandlerDelegate.Setup(x => x()).ReturnsAsync(response);

        //Act
        var result = await _loggingBehavior.Handle(request, requestHandlerDelegate.Object, CancellationToken.None);

        //Assert
        Assert.Equal(result, response);

        _logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => string.Equals(expectedLogBeforeMethod, o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
        _logger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((o, t) => string.Equals(expectedLogAfterMethod, o.ToString(),
                    StringComparison.InvariantCultureIgnoreCase)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    public class TestRequest : IRequest<TestResponse>
    {
        public string Name { get; set; }
    }

    public class TestResponse
    {
        public string Name { get; set; }
    }
}
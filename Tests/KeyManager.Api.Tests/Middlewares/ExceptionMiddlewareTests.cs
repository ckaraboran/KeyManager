namespace KeyManager.Api.Tests.Middlewares;

public class ExceptionMiddlewareTests
{
    private readonly Mock<ILogger<ExceptionMiddleware>> _mockLogger;

    public ExceptionMiddlewareTests()
    {
        _mockLogger = new Mock<ILogger<ExceptionMiddleware>>();
    }

    [Fact]
    public async Task Given_RecordNotFoundException_When_Thrown_Then_ReturnNotFoundHttpCode()
    {
        //Arrange
        var mockProductApiException = new RecordNotFoundException("test");

        Task MockNextMiddleware(HttpContext _)
        {
            return Task.FromException(mockProductApiException);
        }

        var httpContext = new DefaultHttpContext();
        var exceptionHandlingMiddleware = new ExceptionMiddleware(MockNextMiddleware, _mockLogger.Object);

        //Act
        await exceptionHandlingMiddleware.Invoke(httpContext);

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode)httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task Given_RecordCannotBeDeletedException_When_Thrown_Then_ReturnNotFoundHttpCode()
    {
        //Arrange
        var mockProductApiException = new RecordCannotBeDeletedException("test");

        Task MockNextMiddleware(HttpContext _)
        {
            return Task.FromException(mockProductApiException);
        }

        var httpContext = new DefaultHttpContext();
        var exceptionHandlingMiddleware = new ExceptionMiddleware(MockNextMiddleware, _mockLogger.Object);

        //Act
        await exceptionHandlingMiddleware.Invoke(httpContext);

        //Assert
        Assert.Equal(HttpStatusCode.Forbidden, (HttpStatusCode)httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task Given_RecordAlreadyExistsException_When_Thrown_Then_ReturnBadRequestHttpCode()
    {
        //Arrange
        var mockProductApiException = new RecordAlreadyExistsException("test");

        Task MockNextMiddleware(HttpContext _)
        {
            return Task.FromException(mockProductApiException);
        }

        var httpContext = new DefaultHttpContext();
        var exceptionHandlingMiddleware = new ExceptionMiddleware(MockNextMiddleware, _mockLogger.Object);

        //Act
        await exceptionHandlingMiddleware.Invoke(httpContext);

        //Assert
        Assert.Equal(HttpStatusCode.Conflict, (HttpStatusCode)httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task Given_UnauthorizedAccessException_When_Thrown_Then_ReturnUnauthorizedHttpCode()
    {
        //Arrange
        var mockProductApiException = new UnauthorizedAccessException("test");

        Task MockNextMiddleware(HttpContext _)
        {
            return Task.FromException(mockProductApiException);
        }

        var httpContext = new DefaultHttpContext();
        var exceptionHandlingMiddleware = new ExceptionMiddleware(MockNextMiddleware, _mockLogger.Object);

        //Act
        await exceptionHandlingMiddleware.Invoke(httpContext);

        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, (HttpStatusCode)httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task Given_RandomException_When_Thrown_Then_ReturnReturnInternalServerErrorStatusCode()
    {
        //Arrange
        var mockException = new Exception("test");

        Task MockNextMiddleware(HttpContext _)
        {
            return Task.FromException(mockException);
        }

        var httpContext = new DefaultHttpContext();
        var exceptionHandlingMiddleware = new ExceptionMiddleware(MockNextMiddleware, _mockLogger.Object);

        //Act
        await exceptionHandlingMiddleware.Invoke(httpContext);

        //Assert
        Assert.Equal(HttpStatusCode.InternalServerError, (HttpStatusCode)httpContext.Response.StatusCode);
    }
}
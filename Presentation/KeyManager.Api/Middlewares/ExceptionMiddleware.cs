namespace KeyManager.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                case RecordNotFoundException ex:

                    _logger.LogError(
                        "Record not found exception occurred. Error message: '{Message}' Exception stack trace: '{StackTrace}'",
                        ex.Message, ex.StackTrace);
                    response.StatusCode = (int)HttpStatusCode.NotFound;

                    break;
                case RecordAlreadyExistsException ex:

                    _logger.LogError(
                        "Record already exists exception occured. Error message: '{Message}' Exception stack trace: '{StackTrace}'",
                        ex.Message, ex.StackTrace);
                    response.StatusCode = (int)HttpStatusCode.BadRequest;

                    break;
                case UnauthorizedAccessException ex:

                    _logger.LogError(
                        "Unauthorized Access exception occurred. Error message: '{Message}'  Exception stack trace: '{StackTrace}'",
                        ex.Message, ex.StackTrace);
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;

                    break;
                default:

                    _logger.LogError("An error occurred. Exception Message: '{Message}'", error.Message);
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    break;
            }

            var result = JsonConvert.SerializeObject(new { message = error.Message });
            await response.WriteAsync(result);
        }
    }
}
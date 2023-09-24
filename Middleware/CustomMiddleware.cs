using NewsManagementMinimal.Extensions;

namespace NewsManagementMinimal.Middleware;

public class CustomMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public CustomMiddleware(RequestDelegate next, ILogger<CustomMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            LogRequest(context);

            var originalBodyStream = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            LogResponse(context, responseBodyStream);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalBodyStream);
        }
        catch (Exception ex)
        {
            _logger.LogError($"\n{ex.ToJson()}\n");
        }
    }

    private async void LogRequest(HttpContext context)
    {
        var request = context.Request;
        _logger.LogInformation($"\n\nRequest: {request.Method} {request.Path}");

        if (!request.Body.CanSeek) return;
        request.Body.Seek(0, SeekOrigin.Begin);
        var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
        _logger.LogInformation($"Request Body: {requestBody.ToJson()}");
        request.Body.Seek(0, SeekOrigin.Begin);
    }

    private async void LogResponse(HttpContext context, MemoryStream responseBodyStream)
    {
        var response = context.Response;
        _logger.LogInformation($"Response Code: {response.StatusCode}");
        _logger.LogInformation($"Response Headers: {string.Join(", ", response.Headers.ToJson())}");

        responseBodyStream.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(response.Body).ReadToEndAsync();
        _logger.LogInformation($"Response Text: {responseText}\n\n");
    }
}
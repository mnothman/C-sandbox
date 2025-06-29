namespace TaskManagerSandbox.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;
        var requestPath = context.Request.Path;
        var requestMethod = context.Request.Method;
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        _logger.LogInformation("Request started: {Method} {Path} from {IpAddress} - UserAgent: {UserAgent}", 
            requestMethod, requestPath, ipAddress, userAgent);

        try
        {
            await _next(context);
        }
        finally
        {
            var duration = DateTime.UtcNow - startTime;
            var statusCode = context.Response.StatusCode;

            _logger.LogInformation("Request completed: {Method} {Path} - Status: {StatusCode} - Duration: {Duration}ms", 
                requestMethod, requestPath, statusCode, duration.TotalMilliseconds);
        }
    }
} 
using Server.Interfaces;

namespace Server.Middlewares;

public class UnauthorizedMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IServiceScopeFactory serviceScopeFactory)
{
    public async Task Invoke(HttpContext context)
    {
        await next(context);

        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            var message = $"Unauthorized access detected";
            var path = context.Request.Path;
            logger.LogWarning(message);
            await LogInDatabaseLogger(message, path);
        }
    }

    private async Task LogInDatabaseLogger(string message, string path)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var logService = scope.ServiceProvider.GetRequiredService<ILogService>();
        await logService.Log(message, path, "WARNING");
    }
}

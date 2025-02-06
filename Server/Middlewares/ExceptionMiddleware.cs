using Server.Interfaces;
using System.Net;
using System.Text.Json;

namespace Server.Middlewares;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IServiceScopeFactory serviceScopeFactory)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, "Server Error");
            await LogInDatabaseLogger(context, ex);
            await SendResponse(context);
        }
    }

    private async static Task SendResponse(HttpContext context)
    {
        var response = new
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Message = "Server Error",
            Details = ":'v"
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = response.StatusCode;

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
    private async Task LogInDatabaseLogger(HttpContext context, Exception ex)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var logService = scope.ServiceProvider.GetRequiredService<ILogService>();
        await logService.Log(ex.Message, context.Request.Path, "ERROR");
    }
}

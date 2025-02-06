
using Microsoft.Extensions.Options;
using Server.Interfaces;
using Server.Options;

namespace Server.Services;

public class ToDoBackgroundService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _interval;

    public ToDoBackgroundService(IOptions<BackgroundOptions> options, IServiceScopeFactory serviceScopeFactory)
    {
        _scopeFactory = serviceScopeFactory;
        _interval = TimeSpan.FromSeconds(options.Value.IntervalSeconds);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var logService = scope.ServiceProvider.GetService<ILogService>();
            await logService!.Log("BackgroundService Started", "BackgroundService", "INFORMATION");
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var logService = scope.ServiceProvider.GetService<ILogService>();
            var toDoService = scope.ServiceProvider.GetService<IToDoService>();
            try { await toDoService!.OverdueExpiredTasks(); }
            catch (Exception _) { await logService!.Log("Error on background action excecution", "BackgroundService", "ERROR"); }
            await Task.Delay(_interval, stoppingToken);
        }

        using (var scope = _scopeFactory.CreateScope())
        {
            var logService = scope.ServiceProvider.GetService<ILogService>();
            await logService!.Log("BackgroundService Stopped", "BackgroundService", "INFORMATION");
        }
    }
}

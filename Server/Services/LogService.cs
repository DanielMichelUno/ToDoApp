using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Interfaces;
using Server.Models;
using System.Security.Claims;

namespace Server.Services;

public class LogService(ToDoAppContext context, ISessionService sessionService) : ILogService
{
    public async Task Log(string message, string route, string level)
    {
        var user = sessionService.GetClaim(ClaimTypes.Name) ?? "Unauthenticated";
        await context.AddAsync(new LogEntry { UserName = user, Route = route, CreatedAt = DateTime.Now, Message = message, Level = level });
        await context.SaveChangesAsync();
    }
}

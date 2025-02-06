using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Interfaces;

public interface IToDoAppContext : IDisposable
{
    DbSet<ToDoStatus> ToDoStatuses { get; set; }
    DbSet<ToDo> ToDos { get; set; }
    DbSet<LogEntry> LogEntries { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

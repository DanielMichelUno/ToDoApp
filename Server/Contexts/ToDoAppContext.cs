using Microsoft.EntityFrameworkCore;
using Server.Interfaces;
using Server.Models;

namespace Server.Contexts;

public class ToDoAppContext(DbContextOptions<ToDoAppContext> options) : DbContext(options), IToDoAppContext
{
    public DbSet<ToDoStatus> ToDoStatuses { get; set; }
    public DbSet<ToDo> ToDos { get; set; }
    public DbSet<LogEntry> LogEntries { get; set; }
}

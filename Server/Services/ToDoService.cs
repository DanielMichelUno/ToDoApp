using Microsoft.EntityFrameworkCore;
using Server.Dtos;
using Server.Enums;
using Server.Interfaces;
using Server.Models;

namespace Server.Services;

public class ToDoService(IToDoAppContext context) : IToDoService
{
    private readonly ToDoStatuses[] OverduableStatuses = [ToDoStatuses.ToDo, ToDoStatuses.InProgress];
    public async Task<int> CreateToDoTask(CreateToDoDto createDto)
    {
        var todoTask = new ToDo { Description = createDto.Description, StatusId = createDto.StatusId, DueDate = createDto.DueDate };
        context.ToDos.Add(todoTask);
        await context.SaveChangesAsync();
        return todoTask.Id;
    }

    public async Task<IEnumerable<SelectOption<ToDoStatuses>>> GetStatusesOptions()
    {
        return await context.ToDoStatuses
            .Where(x => x.Id != ToDoStatuses.Overdue)
            .Select(x => new SelectOption<ToDoStatuses>(x.Id, x.Name)).ToListAsync();
    }

    public async Task<bool> DeleteToDoTask(int id)
    {
        var toDoTask = await context.ToDos.FindAsync(id);
        if (toDoTask == null) return false;
        context.ToDos.Remove(toDoTask);
        await context.SaveChangesAsync();
        return true;
    }

    public Task<List<ToDoDto>> GetToDoTasks() =>
        context.ToDos.Select(x => new ToDoDto(x.Id, x.Description, x.StatusId, x.DueDate)).ToListAsync();

    public async Task<bool> UpdateToDoTask(ToDoDto updateDto)
    {
        var toDoTask = await context.ToDos.FindAsync(updateDto.Id);
        if (toDoTask == null) return false;
        toDoTask.Description = updateDto.Description;
        toDoTask.DueDate = updateDto.DueDate;
        toDoTask.StatusId = updateDto.StatusId;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task OverdueExpiredTasks()
    {
        var newOverdueTasks = context.ToDos.Where(x => DateTime.Today > x.DueDate && OverduableStatuses.Contains(x.StatusId));
        foreach (var task in newOverdueTasks) task.StatusId = ToDoStatuses.Overdue;
        await context.SaveChangesAsync();
    }
}

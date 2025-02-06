using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Dtos;
using Server.Enums;
using Server.Models;
using Server.Services;

namespace Server.Test.Services;

public class ToDoServiceTest : IDisposable
{
    private readonly ToDoAppContext _context;
    public ToDoServiceTest()
    {
        var options = new DbContextOptionsBuilder<ToDoAppContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        _context = new ToDoAppContext(options);
    }


    [Fact]
    public async Task GetStatusesOptions_OptionsFound_ReturnOptions()
    {
        //Arrange
        ClearContext();
        var statusName = "statusName";
        var statusKey = ToDoStatuses.ToDo;

        _context.ToDoStatuses.Add(new() { Id = statusKey, Name = statusName });
        await _context.SaveChangesAsync();

        var service = new ToDoService(_context);

        //Act
        var results = await service.GetStatusesOptions();

        //Assert
        Assert.NotEmpty(results);
        var status = results.FirstOrDefault();
        Assert.NotNull(status);
        Assert.Equal(status.Text, statusName);
        Assert.Equal(status.Key, statusKey);
    }

    [Fact]
    public async Task CreateToDoTask_ValidInput_TaskCreated()
    {
        //Arrange
        ClearContext();
        var status = ToDoStatuses.ToDo;
        var description = "test description";
        var dueDate = DateTime.Today.AddDays(1);

        var input = new CreateToDoDto(description, status, dueDate);

        var service = new ToDoService(_context);

        //Act
        var result1 = await service.CreateToDoTask(input);
        var result2 = await service.CreateToDoTask(input);

        //Asser
        Assert.True(result1 > 0);
        Assert.True(result2 > result1);
    }

    [Fact]
    public async Task DeleteToDoTask_TaskFound_ReturnTrue()
    {
        //Arrange
        ClearContext();
        var id = 1;
        _context.ToDos.Add(new() { Description = "test", Id = id, DueDate = DateTime.Today.AddDays(1), StatusId = ToDoStatuses.ToDo });
        await _context.SaveChangesAsync();

        var service = new ToDoService(_context);

        //Act
        var result = await service.DeleteToDoTask(id);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteToDoTask_TaskNotFound_ReturnFalse()
    {
        //Arrange
        ClearContext();
        var existingId = 1;
        var notExistingId = existingId + 1;
        _context.ToDos.Add(new() { Description = "test", Id = existingId, DueDate = DateTime.Today.AddDays(1), StatusId = ToDoStatuses.ToDo });
        await _context.SaveChangesAsync();

        var service = new ToDoService(_context);

        //Act
        var result = await service.DeleteToDoTask(notExistingId);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetToDoTasks_TasksFound_ReturnTask()
    {
        //Arrange
        ClearContext();
        _context.ToDos.Add(new() { Description = "test", Id = 1, DueDate = DateTime.Today.AddDays(1), StatusId = ToDoStatuses.ToDo });
        _context.ToDos.Add(new() { Description = "test", Id = 2, DueDate = DateTime.Today.AddDays(1), StatusId = ToDoStatuses.ToDo });
        _context.ToDos.Add(new() { Description = "test", Id = 3, DueDate = DateTime.Today.AddDays(1), StatusId = ToDoStatuses.ToDo });
        _context.ToDos.Add(new() { Description = "test", Id = 4, DueDate = DateTime.Today.AddDays(1), StatusId = ToDoStatuses.ToDo });
        await _context.SaveChangesAsync();

        var service = new ToDoService(_context);

        //Act
        var result = await service.GetToDoTasks();

        //Assert
        Assert.NotEmpty(result);
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public async Task UpdateToDoTask_TaskFound_ReturnTrue()
    {
        //Arrange
        ClearContext();
        var id = 1;
        var todo = new ToDo { Description = "test", Id = id, DueDate = DateTime.Today.AddDays(1), StatusId = ToDoStatuses.ToDo };
        _context.ToDos.Add(todo);
        await _context.SaveChangesAsync();

        var service = new ToDoService(_context);

        var updateDto = new ToDoDto(id, "test modified", ToDoStatuses.InProgress, DateTime.Today.AddDays(2));

        //Act
        var result = await service.UpdateToDoTask(updateDto);

        //Assert
        Assert.True(result);;
    }

    [Fact]
    public async Task UpdateToDoTask_TaskNotFound_ReturnFalse()
    {
        //Arrange
        ClearContext();
        var id = 1;
        var todo = new ToDo { Description = "test", Id = id, DueDate = DateTime.Today.AddDays(1), StatusId = ToDoStatuses.ToDo };
        _context.ToDos.Add(todo);
        await _context.SaveChangesAsync();

        var service = new ToDoService(_context);

        var updateDto = new ToDoDto(id + 1, "test modified", ToDoStatuses.InProgress, DateTime.Today.AddDays(2));

        //Act
        var result = await service.UpdateToDoTask(updateDto);

        //Assert
        Assert.False(result); ;
    }

    [Fact]
    public async Task OverdueExpiredTasks_SomeTaskHaveExpired_SetExpiredTaskStatusToOverdue()
    {
        //Assert
        ClearContext();
        var expiredTaskId = 1; 
        var notExpiredTaskId = 2;
        _context.ToDos.Add(new() { Description = "test", Id = expiredTaskId, DueDate = DateTime.Today.AddDays(-1), StatusId = ToDoStatuses.ToDo });
        _context.ToDos.Add(new() { Description = "test", Id = notExpiredTaskId, DueDate = DateTime.Today.AddDays(1), StatusId = ToDoStatuses.ToDo });
        await _context.SaveChangesAsync();

        var service = new ToDoService(_context);

        //Act
        await service.OverdueExpiredTasks();

        //Assert
        var expiredTasks = await _context.ToDos.Where(x => x.StatusId == ToDoStatuses.Overdue).ToListAsync();
        var notExpiredTasks = await _context.ToDos.Where(x => x.StatusId != ToDoStatuses.Overdue).ToListAsync();
        Assert.Single(expiredTasks);
        Assert.Single(notExpiredTasks);
        var expiredTask = expiredTasks.FirstOrDefault();
        var notExpiredTask = notExpiredTasks.FirstOrDefault();
        Assert.Equal(expiredTaskId, expiredTask!.Id);
        Assert.Equal(notExpiredTaskId, notExpiredTask!.Id);
    }

    private void ClearContext()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}

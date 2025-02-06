using Server.Dtos;
using Server.Enums;

namespace Server.Interfaces;

public interface IToDoService
{
    Task<IEnumerable<SelectOption<ToDoStatuses>>> GetStatusesOptions();
    Task<int> CreateToDoTask(CreateToDoDto createDto);
    Task<bool> DeleteToDoTask(int id);
    Task<List<ToDoDto>> GetToDoTasks();
    Task<bool> UpdateToDoTask(ToDoDto updateDto);
    Task OverdueExpiredTasks();
}

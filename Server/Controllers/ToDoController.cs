using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Dtos;
using Server.Interfaces;

namespace Server.Controllers;

[Route("todo")]
[ApiController]
public class ToDoController(IToDoService service) : ControllerBase
{
    [HttpGet]
    [Route("statuses")]
    [Authorize]
    public async Task<ActionResult> GetToDoStatuses() =>
        Ok(await service.GetStatusesOptions());

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateToDoTask(CreateToDoDto toDoDto) =>
        Ok(await service.CreateToDoTask(toDoDto));

    [HttpDelete]
    [Route("{id}")]
    [Authorize]
    public async Task<ActionResult> DeleteToDoTask(int id)
    {
        var deleted = await service.DeleteToDoTask(id);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPut]
    [Authorize]
    public async Task<ActionResult> UpdateToDoTask([FromBody]ToDoDto updateDto)
    {
        var updated = await service.UpdateToDoTask(updateDto);
        return updated ? NoContent() : NotFound();
    }

    [HttpGet]
    [Route("error")]
    [Authorize]
    public ActionResult TestError()
    {
        throw new Exception("detalles del error confidencial :v");
    }

    [HttpPut]
    [Route("overdue")]
    [Authorize]
    public async Task<ActionResult> OverdueExpiredTasks()
    {
        await service.OverdueExpiredTasks();
        return NoContent();
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult> GetToDoList()
    {
        return Ok(await service.GetToDoTasks());
    }
}

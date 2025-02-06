using Server.Enums;
using System.ComponentModel.DataAnnotations;

namespace Server.Dtos;

public record ToDoDto
(
    int Id,
    [StringLength(250)] string Description,
    ToDoStatuses StatusId,
    DateTime? DueDate
);

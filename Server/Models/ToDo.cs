using Server.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public class ToDo
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(250)]
    public string Description { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }

    [Required]
    public ToDoStatuses StatusId { get; set; }

    [ForeignKey(nameof(StatusId))]
    public virtual ToDoStatus Status { get; set; } = null!;
}

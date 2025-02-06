using Server.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

[Table("ToDoStatuses")]
public class ToDoStatus
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public ToDoStatuses Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
}

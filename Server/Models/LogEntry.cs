using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public class LogEntry
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(1000)]
    public string Message { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    [StringLength(250)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [StringLength(250)]
    public string Route { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Level {  get; set; } = string.Empty;
}

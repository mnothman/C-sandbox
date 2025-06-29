using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagerSandbox.Models;

public class TaskItem
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    [Required]
    public TaskItemStatus Status { get; set; } = TaskItemStatus.Pending;
    
    [Required]
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    
    public DateTime? DueDate { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? CompletedAt { get; set; }
    
    [StringLength(100)]
    public string? AssignedTo { get; set; }
    
    [StringLength(100)]
    public string CreatedBy { get; set; } = string.Empty;
    
    public List<string> Tags { get; set; } = new();
    
    public int? EstimatedHours { get; set; }
    
    public int? ActualHours { get; set; }
    
    [StringLength(500)]
    public string? Notes { get; set; }
    
    // Navigation properties for future relationships
    public int? UserId { get; set; }
    public virtual User? User { get; set; }
    
    public int? CategoryId { get; set; }
    public virtual Category? Category { get; set; }
}

public enum TaskItemStatus
{
    Pending,
    InProgress,
    Completed,
    Cancelled,
    OnHold
}

public enum TaskPriority
{
    Low,
    Medium,
    High,
    Critical
} 
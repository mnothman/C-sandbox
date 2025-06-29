using System.ComponentModel.DataAnnotations;

namespace TaskManagerSandbox.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [StringLength(7)]
    public string? Color { get; set; } = "#007bff"; // Default blue color
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Foreign key relationships
    public int CreatedById { get; set; }
    public virtual User CreatedBy { get; set; } = null!;
    
    // Navigation properties
    public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
} 
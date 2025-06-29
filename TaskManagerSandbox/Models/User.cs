using System.ComponentModel.DataAnnotations;

namespace TaskManagerSandbox.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    [StringLength(20)]
    public string? PhoneNumber { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? LastLoginAt { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public UserRole Role { get; set; } = UserRole.User;
    
    [StringLength(500)]
    public string? Bio { get; set; }
    
    public string? ProfilePictureUrl { get; set; }
    
    // Navigation properties
    public virtual ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}

public enum UserRole
{
    User,
    Manager,
    Admin
} 
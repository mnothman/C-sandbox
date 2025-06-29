namespace TaskManagerSandbox.DTOs;

public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? AssignedTo { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public int? EstimatedHours { get; set; }
    public int? ActualHours { get; set; }
    public string? Notes { get; set; }
    public string? CategoryName { get; set; }
    public string? CategoryColor { get; set; }
}

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Priority { get; set; } = "Medium";
    public DateTime? DueDate { get; set; }
    public string? AssignedTo { get; set; }
    public List<string> Tags { get; set; } = new();
    public int? EstimatedHours { get; set; }
    public string? Notes { get; set; }
    public int? CategoryId { get; set; }
}

public class UpdateTaskDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public string? AssignedTo { get; set; }
    public List<string>? Tags { get; set; }
    public int? EstimatedHours { get; set; }
    public int? ActualHours { get; set; }
    public string? Notes { get; set; }
    public int? CategoryId { get; set; }
}

public class TaskFilterDto
{
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public string? AssignedTo { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? DueDateFrom { get; set; }
    public DateTime? DueDateTo { get; set; }
    public List<string>? Tags { get; set; }
    public int? CategoryId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; } = "CreatedAt";
    public bool SortDescending { get; set; } = true;
} 
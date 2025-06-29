using TaskManagerSandbox.Models;

namespace TaskManagerSandbox.Data;

public static class SeedData
{
    public static async Task InitializeAsync(TaskManagerContext context)
    {
        // Check if data already exists
        if (context.Tasks.Any())
        {
            return;
        }

        // Get existing users and categories
        var adminUser = await context.Users.FindAsync(1);
        var workCategory = await context.Categories.FindAsync(1);
        var personalCategory = await context.Categories.FindAsync(2);
        var urgentCategory = await context.Categories.FindAsync(3);

        if (adminUser == null || workCategory == null)
        {
            return; // Seed data not properly initialized
        }

        // Create sample tasks
        var tasks = new List<TaskItem>
        {
            new TaskItem
            {
                Title = "Complete API Documentation",
                Description = "Write comprehensive API documentation for the Task Manager application",
                Status = TaskItemStatus.InProgress,
                Priority = TaskPriority.High,
                DueDate = DateTime.UtcNow.AddDays(7),
                AssignedTo = "admin",
                CreatedBy = "admin",
                Tags = new List<string> { "documentation", "api", "important" },
                EstimatedHours = 8,
                Notes = "Include examples for all endpoints",
                CategoryId = workCategory.Id,
                UserId = adminUser.Id
            },
            new TaskItem
            {
                Title = "Review Code Changes",
                Description = "Review pull requests and provide feedback to team members",
                Status = TaskItemStatus.Pending,
                Priority = TaskPriority.Medium,
                DueDate = DateTime.UtcNow.AddDays(2),
                AssignedTo = "admin",
                CreatedBy = "admin",
                Tags = new List<string> { "review", "code", "team" },
                EstimatedHours = 4,
                CategoryId = workCategory.Id,
                UserId = adminUser.Id
            },
            new TaskItem
            {
                Title = "Plan Team Meeting",
                Description = "Schedule and prepare agenda for weekly team meeting",
                Status = TaskItemStatus.Completed,
                Priority = TaskPriority.Low,
                DueDate = DateTime.UtcNow.AddDays(-1),
                AssignedTo = "admin",
                CreatedBy = "admin",
                Tags = new List<string> { "meeting", "planning" },
                EstimatedHours = 2,
                ActualHours = 1,
                CompletedAt = DateTime.UtcNow.AddDays(-1),
                CategoryId = workCategory.Id,
                UserId = adminUser.Id
            },
            new TaskItem
            {
                Title = "Buy Groceries",
                Description = "Purchase groceries for the week",
                Status = TaskItemStatus.Pending,
                Priority = TaskPriority.Medium,
                DueDate = DateTime.UtcNow.AddDays(1),
                AssignedTo = "user",
                CreatedBy = "user",
                Tags = new List<string> { "personal", "shopping" },
                EstimatedHours = 1,
                CategoryId = personalCategory.Id
            },
            new TaskItem
            {
                Title = "Fix Critical Bug",
                Description = "Urgent fix needed for production issue",
                Status = TaskItemStatus.InProgress,
                Priority = TaskPriority.Critical,
                DueDate = DateTime.UtcNow.AddHours(4),
                AssignedTo = "admin",
                CreatedBy = "admin",
                Tags = new List<string> { "bug", "critical", "production" },
                EstimatedHours = 3,
                CategoryId = urgentCategory.Id,
                UserId = adminUser.Id
            },
            new TaskItem
            {
                Title = "Exercise",
                Description = "Go for a 30-minute run",
                Status = TaskItemStatus.Pending,
                Priority = TaskPriority.Low,
                DueDate = DateTime.UtcNow.AddDays(1),
                AssignedTo = "user",
                CreatedBy = "user",
                Tags = new List<string> { "health", "exercise" },
                EstimatedHours = 1,
                CategoryId = personalCategory.Id
            }
        };

        await context.Tasks.AddRangeAsync(tasks);
        await context.SaveChangesAsync();
    }
} 
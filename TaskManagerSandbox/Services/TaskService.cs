using TaskManagerSandbox.Data;
using TaskManagerSandbox.DTOs;
using TaskManagerSandbox.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace TaskManagerSandbox.Services;

public class TaskService : ITaskService
{
    private readonly TaskManagerContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<TaskService> _logger;

    public TaskService(TaskManagerContext context, IMapper mapper, ILogger<TaskService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
    {
        var tasks = await _context.Tasks
            .Include(t => t.Category)
            .Include(t => t.User)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public async Task<TaskDto?> GetTaskByIdAsync(int id)
    {
        var task = await _context.Tasks
            .Include(t => t.Category)
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id);

        return _mapper.Map<TaskDto>(task);
    }

    public async Task<IEnumerable<TaskDto>> GetTasksByFilterAsync(TaskFilterDto filter)
    {
        var query = _context.Tasks
            .Include(t => t.Category)
            .Include(t => t.User)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrEmpty(filter.Status))
        {
            if (Enum.TryParse<TaskItemStatus>(filter.Status, true, out var status))
            {
                query = query.Where(t => t.Status == status);
            }
        }

        if (!string.IsNullOrEmpty(filter.Priority))
        {
            if (Enum.TryParse<TaskPriority>(filter.Priority, true, out var priority))
            {
                query = query.Where(t => t.Priority == priority);
            }
        }

        if (!string.IsNullOrEmpty(filter.AssignedTo))
        {
            query = query.Where(t => t.AssignedTo == filter.AssignedTo);
        }

        if (!string.IsNullOrEmpty(filter.CreatedBy))
        {
            query = query.Where(t => t.CreatedBy == filter.CreatedBy);
        }

        if (filter.DueDateFrom.HasValue)
        {
            query = query.Where(t => t.DueDate >= filter.DueDateFrom);
        }

        if (filter.DueDateTo.HasValue)
        {
            query = query.Where(t => t.DueDate <= filter.DueDateTo);
        }

        if (filter.Tags?.Any() == true)
        {
            query = query.Where(t => t.Tags.Any(tag => filter.Tags!.Contains(tag)));
        }

        if (filter.CategoryId.HasValue)
        {
            query = query.Where(t => t.CategoryId == filter.CategoryId);
        }

        // Apply sorting
        query = filter.SortBy?.ToLower() switch
        {
            "title" => filter.SortDescending ? query.OrderByDescending(t => t.Title) : query.OrderBy(t => t.Title),
            "duedate" => filter.SortDescending ? query.OrderByDescending(t => t.DueDate) : query.OrderBy(t => t.DueDate),
            "priority" => filter.SortDescending ? query.OrderByDescending(t => t.Priority) : query.OrderBy(t => t.Priority),
            "status" => filter.SortDescending ? query.OrderByDescending(t => t.Status) : query.OrderBy(t => t.Status),
            _ => filter.SortDescending ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt)
        };

        // Apply pagination
        var tasks = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto, string createdBy)
    {
        var task = new TaskItem
        {
            Title = createTaskDto.Title,
            Description = createTaskDto.Description,
            Priority = Enum.Parse<TaskPriority>(createTaskDto.Priority, true),
            DueDate = createTaskDto.DueDate,
            AssignedTo = createTaskDto.AssignedTo,
            Tags = createTaskDto.Tags,
            EstimatedHours = createTaskDto.EstimatedHours,
            Notes = createTaskDto.Notes,
            CategoryId = createTaskDto.CategoryId,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Task created: {TaskId} by {CreatedBy}", task.Id, createdBy);

        return await GetTaskByIdAsync(task.Id) ?? throw new InvalidOperationException("Failed to retrieve created task");
    }

    public async Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return null;
        }

        // Update properties if provided
        if (!string.IsNullOrEmpty(updateTaskDto.Title))
            task.Title = updateTaskDto.Title;

        if (updateTaskDto.Description != null)
            task.Description = updateTaskDto.Description;

        if (!string.IsNullOrEmpty(updateTaskDto.Status))
            task.Status = Enum.Parse<TaskItemStatus>(updateTaskDto.Status, true);

        if (!string.IsNullOrEmpty(updateTaskDto.Priority))
            task.Priority = Enum.Parse<TaskPriority>(updateTaskDto.Priority, true);

        if (updateTaskDto.DueDate.HasValue)
            task.DueDate = updateTaskDto.DueDate;

        if (updateTaskDto.AssignedTo != null)
            task.AssignedTo = updateTaskDto.AssignedTo;

        if (updateTaskDto.Tags != null)
            task.Tags = updateTaskDto.Tags;

        if (updateTaskDto.EstimatedHours.HasValue)
            task.EstimatedHours = updateTaskDto.EstimatedHours;

        if (updateTaskDto.ActualHours.HasValue)
            task.ActualHours = updateTaskDto.ActualHours;

        if (updateTaskDto.Notes != null)
            task.Notes = updateTaskDto.Notes;

        if (updateTaskDto.CategoryId.HasValue)
            task.CategoryId = updateTaskDto.CategoryId;

        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Task updated: {TaskId}", id);

        return await GetTaskByIdAsync(id);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return false;
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Task deleted: {TaskId}", id);
        return true;
    }

    public async Task<bool> CompleteTaskAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return false;
        }

        task.Status = TaskItemStatus.Completed;
        task.CompletedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _logger.LogInformation("Task completed: {TaskId}", id);
        return true;
    }

    public async Task<IEnumerable<TaskDto>> GetTasksByUserAsync(string username)
    {
        var tasks = await _context.Tasks
            .Include(t => t.Category)
            .Include(t => t.User)
            .Where(t => t.AssignedTo == username || t.CreatedBy == username)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TaskDto>>(tasks);
    }

    public async Task<IEnumerable<TaskDto>> GetOverdueTasksAsync()
    {
        var overdueTasks = await _context.Tasks
            .Include(t => t.Category)
            .Include(t => t.User)
            .Where(t => t.DueDate.HasValue && 
                       t.DueDate < DateTime.UtcNow && 
                       t.Status != TaskItemStatus.Completed && 
                       t.Status != TaskItemStatus.Cancelled)
            .OrderBy(t => t.DueDate)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TaskDto>>(overdueTasks);
    }

    public async Task<object> GetTaskStatisticsAsync()
    {
        var totalTasks = await _context.Tasks.CountAsync();
        var completedTasks = await _context.Tasks.CountAsync(t => t.Status == TaskItemStatus.Completed);
        var pendingTasks = await _context.Tasks.CountAsync(t => t.Status == TaskItemStatus.Pending);
        var inProgressTasks = await _context.Tasks.CountAsync(t => t.Status == TaskItemStatus.InProgress);
        var overdueTasks = await _context.Tasks.CountAsync(t => 
            t.DueDate.HasValue && 
            t.DueDate < DateTime.UtcNow && 
            t.Status != TaskItemStatus.Completed && 
            t.Status != TaskItemStatus.Cancelled);

        var priorityStats = await _context.Tasks
            .GroupBy(t => t.Priority)
            .Select(g => new { Priority = g.Key.ToString(), Count = g.Count() })
            .ToListAsync();

        var categoryStats = await _context.Tasks
            .GroupBy(t => t.Category.Name)
            .Select(g => new { Category = g.Key ?? "Uncategorized", Count = g.Count() })
            .ToListAsync();

        return new
        {
            TotalTasks = totalTasks,
            CompletedTasks = completedTasks,
            PendingTasks = pendingTasks,
            InProgressTasks = inProgressTasks,
            OverdueTasks = overdueTasks,
            CompletionRate = totalTasks > 0 ? (double)completedTasks / totalTasks * 100 : 0,
            PriorityBreakdown = priorityStats,
            CategoryBreakdown = categoryStats
        };
    }
} 
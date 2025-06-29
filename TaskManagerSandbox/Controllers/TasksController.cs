using Microsoft.AspNetCore.Mvc;
using TaskManagerSandbox.DTOs;
using TaskManagerSandbox.Services;
using FluentValidation;

namespace TaskManagerSandbox.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly ILogger<TasksController> _logger;

    public TasksController(ITaskService taskService, ILogger<TasksController> logger)
    {
        _taskService = taskService;
        _logger = logger;
    }

    /// <summary>
    /// Get all tasks
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    /// <summary>
    /// Get a specific task by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetTask(int id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound(new { Message = $"Task with ID {id} not found" });
        }

        return Ok(task);
    }

    /// <summary>
    /// Get tasks with filtering, sorting, and pagination
    /// </summary>
    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByFilter([FromQuery] TaskFilterDto filter)
    {
        var tasks = await _taskService.GetTasksByFilterAsync(filter);
        return Ok(tasks);
    }

    /// <summary>
    /// Create a new task
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TaskDto>> CreateTask(CreateTaskDto createTaskDto)
    {
        try
        {
            // For now, we'll use a default user. In a real app, this would come from authentication
            var createdBy = "admin";
            var task = await _taskService.CreateTaskAsync(createTaskDto, createdBy);
            
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { Message = "Validation failed", Errors = ex.Errors });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task");
            return StatusCode(500, new { Message = "An error occurred while creating the task" });
        }
    }

    /// <summary>
    /// Update an existing task
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<TaskDto>> UpdateTask(int id, UpdateTaskDto updateTaskDto)
    {
        var task = await _taskService.UpdateTaskAsync(id, updateTaskDto);
        if (task == null)
        {
            return NotFound(new { Message = $"Task with ID {id} not found" });
        }

        return Ok(task);
    }

    /// <summary>
    /// Delete a task
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTask(int id)
    {
        var success = await _taskService.DeleteTaskAsync(id);
        if (!success)
        {
            return NotFound(new { Message = $"Task with ID {id} not found" });
        }

        return NoContent();
    }

    /// <summary>
    /// Mark a task as completed
    /// </summary>
    [HttpPatch("{id}/complete")]
    public async Task<ActionResult> CompleteTask(int id)
    {
        var success = await _taskService.CompleteTaskAsync(id);
        if (!success)
        {
            return NotFound(new { Message = $"Task with ID {id} not found" });
        }

        return Ok(new { Message = "Task marked as completed" });
    }

    /// <summary>
    /// Get tasks assigned to a specific user
    /// </summary>
    [HttpGet("user/{username}")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByUser(string username)
    {
        var tasks = await _taskService.GetTasksByUserAsync(username);
        return Ok(tasks);
    }

    /// <summary>
    /// Get overdue tasks
    /// </summary>
    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetOverdueTasks()
    {
        var tasks = await _taskService.GetOverdueTasksAsync();
        return Ok(tasks);
    }

    /// <summary>
    /// Get task statistics
    /// </summary>
    [HttpGet("statistics")]
    public async Task<ActionResult<object>> GetTaskStatistics()
    {
        var statistics = await _taskService.GetTaskStatisticsAsync();
        return Ok(statistics);
    }
} 
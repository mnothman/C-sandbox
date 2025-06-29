using TaskManagerSandbox.DTOs;

namespace TaskManagerSandbox.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskDto>> GetAllTasksAsync();
    Task<TaskDto?> GetTaskByIdAsync(int id);
    Task<IEnumerable<TaskDto>> GetTasksByFilterAsync(TaskFilterDto filter);
    Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto, string createdBy);
    Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto);
    Task<bool> DeleteTaskAsync(int id);
    Task<bool> CompleteTaskAsync(int id);
    Task<IEnumerable<TaskDto>> GetTasksByUserAsync(string username);
    Task<IEnumerable<TaskDto>> GetOverdueTasksAsync();
    Task<object> GetTaskStatisticsAsync();
} 